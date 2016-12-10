using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Bespoke.Sph.Domain;
using Bespoke.Sph.WebApi;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;


[RoutePrefix("api/rts-dashboard")]
public class RtsDashboadController : BaseApiController
{
               
    private readonly HttpClient m_client = new HttpClient { BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost) };
    private readonly HttpClient m_rabbitMqManagementClient = new HttpClient(new HttpClientHandler { Credentials = new NetworkCredential(ConfigurationManager.RabbitMqUserName, ConfigurationManager.RabbitMqPassword) })
                                                                { 
                                                                        BaseAddress = new Uri($"{ConfigurationManager.RabbitMqManagementScheme}://{ConfigurationManager.RabbitMqHost}:{ConfigurationManager.RabbitMqManagementPort}") 
                                                                };

    [Route("")]
    [HttpPost]
    public async Task<IHttpActionResult> Aggregates([RawBody]string query)
    {
        var response = await m_client.PostAsync($"{ConfigurationManager.ElasticSearchIndex}_rts/_search", new StringContent(query));
        var content = response.Content as StreamContent;
        if (null == content) throw new Exception("Cannot execute query on es ");
        var result = await content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Cannot execute query for :  {query}, Result = {result} ");
        return Json(result);
    }

    [Route("{type}")]
    [HttpPost]
    public async Task<IHttpActionResult> SearchAsync(string type,
        [RawBody]string query)
    {
        var response = await m_client.PostAsync($"{ConfigurationManager.ElasticSearchIndex}_rts/{type.ToLowerInvariant()}/_search", new StringContent(query));
        var content = response.Content as StreamContent;
        if (null == content) throw new Exception("Cannot execute query on es ");
        var result = await content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            return Invalid((HttpStatusCode) 422, new {query, result});
        return Json(result);
    }

    [Route("dlq")]
    [HttpGet]
    public async Task<IHttpActionResult> GetDlqAsync()
    {
        await Task.Delay(500);
        var response = await m_rabbitMqManagementClient.GetStringAsync("/api/queues/PosEntt/ms_dead_letter_queue");
        return Json(response);
    }

    [Route("logs/{type}/{id}")]
    [HttpGet]
    public async Task<IHttpActionResult> GetSubscriberErrorAsync(string type, string id)
    {
        var query = $@"{{
   ""filter"": {{
      ""query"": {{
         ""bool"": {{
            ""must"": [
               {{
                  ""term"": {{
                     ""otherInfo.id2"": ""{id.Replace("-", "")}""
                  }}
               }},               
               {{
                  ""term"": {{
                     ""otherInfo.type"": ""{type.ToLowerInvariant()}""
                  }}
               }}
            ]
         }}
      }}
   }},
   ""sort"": [
      {{
         ""time"": {{
            ""order"": ""desc""
         }}
      }}
   ]
}}";
        var response = await m_client.PostAsync($"{ConfigurationManager.ElasticSearchIndex}_logs/log/_search", new StringContent(query));
        var content = response.Content as StreamContent;
        if (null == content) throw new Exception("Cannot execute query on es ");
        var result = await content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Cannot execute query for :  {query}, Result = {result} ");
        return Json(result);
    }


    [Route("{type}/requeue/{queues}")]
    [HttpPost]
    public async Task<IHttpActionResult> RequeueAsync(string type, string queues, [JsonBody]JArray items)
    {
        var triggers = queues.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var q in triggers)
        {
            await this.SendMessage(q, items, new Dictionary<string, object>());
        }
        return Json(new { message = "OK", routing = triggers });
    }

    public const int PERSISTENT_DELIVERY_MODE = 2;
    private async Task SendMessage(string triggerId, JArray items, IDictionary<string, object> headers)
    {
        var factory = new ConnectionFactory
        {
            UserName = ConfigurationManager.RabbitMqUserName,
            Password = ConfigurationManager.RabbitMqPassword,
            HostName = ConfigurationManager.RabbitMqHost,
            Port = ConfigurationManager.RabbitMqPort,
            VirtualHost = ConfigurationManager.RabbitMqVirtualHost
        };

        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            foreach (var item in items)
            {
                var log = string.Empty;
                var routingKey = $"trigger_subs_{triggerId}";
                var body = await CompressAsync(item.ToString());

                var props = channel.CreateBasicProperties();
                props.DeliveryMode = PERSISTENT_DELIVERY_MODE;
                props.Persistent = true;
                props.ContentType = "application/json";
                props.Headers = new Dictionary<string, object> { { "operation", "requeue" }, { "crud", "added" }, { "log", log } };
                if (null != headers)
                {
                    foreach (var k in headers.Keys)
                    {
                        if (!props.Headers.ContainsKey(k))
                            props.Headers.Add(k, headers[k]);
                    }
                }
                channel.BasicPublish("", routingKey, props, body);
            }


            channel.Close();
            connection.Close();
        }
    }



    private async Task<byte[]> CompressAsync(string value)
    {
        var content = new byte[value.Length];
        int index = 0;
        foreach (char item in value)
        {
            content[index++] = (byte)item;
        }


        using (var ms = new MemoryStream())
        using (var sw = new GZipStream(ms, CompressionMode.Compress))
        {
            await sw.WriteAsync(content, 0, content.Length);
            //NOTE : DO NOT FLUSH cause bytes will go missing...
            sw.Close();

            content = ms.ToArray();
        }
        return content;
    }


}