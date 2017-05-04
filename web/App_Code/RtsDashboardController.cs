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
using RabbitMQ.Client;
using Newtonsoft.Json.Linq;

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


    [Route("search/{consignmentNo}")]
    [HttpGet]
    public async Task<IHttpActionResult> SearchConsignmentAsync(string consignmentNo)
    {
        var query = $@"{{
    ""query"": {{
        ""query_string"": {{
           ""default_field"": ""_all"",
           ""query"": ""{consignmentNo}""
        }}
    }},
    ""sort"": [
        {{
            ""CreatedDate"": {{
            ""order"": ""asc""
            }}
        }}
    ]
}}";
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
            return Invalid((HttpStatusCode)422, new { query, result });
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


    [Route("{type}/{id}/requeue")]
    [HttpPost]
    public async Task<IHttpActionResult> RequeueAsync(string type, string id, [FromBody]RequeueViewModel model)
    {
        var hit = await m_client.GetStringAsync($"posentt_rts_{model.Date:yyyyMMdd}/{type.ToLowerInvariant()}/{id}");
        var json = JObject.Parse(hit);
        var source = json.SelectToken("$._source").ToString();
        var queueName = model.QueueName ?? "persistence";
        await this.SendMessage(queueName, source, new Dictionary<string, object>());

        if (string.IsNullOrWhiteSpace(model.LogId))
        {
            return Json(new { message = "OK" });
        }

        var update = $@"{{
   ""doc"": {{
      ""otherInfo"": {{
         ""requeued"": true,
         ""requeuedOn"": ""{DateTime.Now:O}"",
         ""requeuedBy"": ""{User.Identity.Name}""
      }}
   }}
}}";
        var response = await m_client.PostAsync($"posentt_logs_{model.Date:yyyyMMdd}/log/{model.LogId}/_update", new StringContent(update));
        response.EnsureSuccessStatusCode();

        return Json(new { message = "OK" });
    }

    public const int PERSISTENT_DELIVERY_MODE = 2;
    private async Task SendMessage(string routingKey, string payload, IDictionary<string, object> headers)
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

            var log = string.Empty;
            var body = await CompressAsync(payload);

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

public class RequeueViewModel
{
    public DateTime Date { set; get; }
    public string LogId { get; set; }
    public string QueueName { get; set; }
}