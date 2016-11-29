using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SubscribersInfrastructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Polly;
using RabbitMQ.Client;

namespace Bespoke.PostEntt.Subscribers
{
    public class ElasticsearchLogsSubscriber : Subscriber
    {
        public override string QueueName => "rts_logs";
        public override string[] RoutingKeys => new[] { "persistence" };
        private TaskCompletionSource<bool> m_stoppingTcs;
        private readonly HttpClient m_client = new HttpClient { BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost) };

        public override void Run(IConnection connection)
        {
            var sw = new Stopwatch();
            sw.Start();

            RegisterServices();

            this.CreateElasticsearchTemplateAsync()
                .ContinueWith(_ =>
                {
                    if (_.IsFaulted)
                    {
                        var entry = new LogEntry(_.Exception);
                        this.WriteError(_.Exception);
                        ObjectBuilder.GetObject<ILogger>().Log(entry);
                        return;
                    }
                    m_stoppingTcs = new TaskCompletionSource<bool>();
                    this.StartConsume(connection);
                    PrintSubscriberInformation(sw.Elapsed);
                    sw.Stop();

                })
                .Wait(5000);
        }

        protected override void OnStop()
        {
            this.WriteMessage("!!Stoping : {0}", this.QueueName);

            m_consumer.Received -= Received;
            m_stoppingTcs?.SetResult(true);

            while (m_processing > 0)
            {

            }
            if (null != m_channel)
            {
                m_channel.Close();
                m_channel.Dispose();
                m_channel = null;
            }

            this.WriteMessage("!!Stopped : {0}", this.QueueName);
        }

        private async Task CreateElasticsearchTemplateAsync()
        {
            string uri = $"_template/{ConfigurationManager.ElasticSearchIndex}_rts";
            var getResponse = await m_client.GetAsync(uri);
            if (getResponse.StatusCode != HttpStatusCode.NotFound) return;

            var context = new SphDataContext();
            var entityDefinitions = context.LoadFromSources<EntityDefinition>();
            var mappings = from ed in entityDefinitions
                           where !ExcludeTypes.Contains(ed.Name)
                           let map = ed.GetElasticsearchMapping().Trim()
                           let trim = map.Trim()
                           select trim.Substring(1, trim.Length - 2).Trim();

            var template = $@"
{{
  ""template"": ""{ConfigurationManager.ElasticSearchIndex}_rts_*"",
  ""aliases"": {{
    ""{ConfigurationManager.ElasticSearchIndex}_rts"": {{}}
  }},
  ""mappings"": {{
        {mappings.ToString(",\r\n")}
  }}
}}";
            var response = await m_client.PutAsync(uri, new StringContent(template));
            response.EnsureSuccessStatusCode();

        }

        private IModel m_channel;
        private TaskBasicConsumer m_consumer;
        private int m_processing;

        public void StartConsume(IConnection connection)
        {
            const bool NO_ACK = false;
            const string EXCHANGE_NAME = "sph.topic";
            const string DEAD_LETTER_EXCHANGE = "sph.ms-dead-letter";
            const string DEAD_LETTER_QUEUE = "ms_dead_letter_queue";

            this.OnStart();

            m_channel = connection.CreateModel();


            m_channel.ExchangeDeclare(EXCHANGE_NAME, ExchangeType.Topic, true);

            m_channel.ExchangeDeclare(DEAD_LETTER_EXCHANGE, ExchangeType.Topic, true);
            var args = new Dictionary<string, object> { { "x-dead-letter-exchange", DEAD_LETTER_EXCHANGE } };
            m_channel.QueueDeclare(this.QueueName, true, false, false, args);

            m_channel.QueueDeclare(DEAD_LETTER_QUEUE, true, false, false, args);
            m_channel.QueueBind(DEAD_LETTER_QUEUE, DEAD_LETTER_EXCHANGE, "#", null);


            foreach (var s in this.RoutingKeys)
            {
                m_channel.QueueBind(this.QueueName, EXCHANGE_NAME, s, null);
            }
            m_channel.BasicQos(0, this.PrefetchCount, false);

            m_consumer = new TaskBasicConsumer(m_channel);
            m_consumer.Received += Received;
            m_channel.BasicConsume(this.QueueName, NO_ACK, m_consumer);

        }

        public static string[] ExcludeTypes = { "SalesOrder", "SapFiAccount", "SurchargeAddOn", "Product", "ItemCategory" };
        private async void Received(object sender, ReceivedMessageArgs e)
        {
            Interlocked.Increment(ref m_processing);
            var body = e.Body;
            var json = await DecompressAsync(body);
            var jo = JObject.Parse(json);
            var entities = jo.SelectToken("$.attached").Select(t => t.ToString().DeserializeFromJson<Entity>()).ToList();

            try
            {
                var tasks = from xc in entities.Where(x => null != x)
                            let type = xc.GetType()
                            where !ExcludeTypes.Contains(type.Name)
                            select ProcessMessageAsync(xc);

                await Task.WhenAll(tasks);
                m_channel.BasicAck(e.DeliveryTag, false);
            }
            catch (Exception exc)
            {
                this.WriteMessage("Error in {0}", this.GetType().Name);
                this.WriteError(exc);
                m_channel.BasicReject(e.DeliveryTag, false);
            }
            finally
            {
                Interlocked.Decrement(ref m_processing);
            }
        }

        private async Task ProcessMessageAsync(Entity item)
        {
            var setting = new JsonSerializerSettings();
            var json = JsonConvert.SerializeObject(item, setting);

            var content = new StringContent(json);
            if (item.IsSystemType()) return;// just custom entity

            var type1 = item.GetType();
            var type = type1.Name.ToLowerInvariant();
            var date = DateTime.Now.ToString(ConfigurationManager.RequestLogIndexPattern);
            var index = $"{ConfigurationManager.ElasticSearchIndex}_rts_{date}";
            var url = $"{ConfigurationManager.ElasticSearchHost}/{index}/{type}/{item.Id}";
            
            
            await Policy.Handle<HttpRequestException>()
                .WaitAndRetryAsync(5, x => TimeSpan.FromMilliseconds(500 * Math.Pow(2, x)))
                .ExecuteAndCaptureAsync(async () => await m_client.PutAsync(url, content));
            this.WriteMessage($"{item.GetType().Name} ...");

        }

        private static async Task<string> DecompressAsync(byte[] content)
        {
            using (var orginalStream = new MemoryStream(content))
            using (var destinationStream = new MemoryStream())
            using (var gzip = new GZipStream(orginalStream, CompressionMode.Decompress))
            {
                try
                {
                    await gzip.CopyToAsync(destinationStream);
                }
                catch (InvalidDataException)
                {
                    orginalStream.CopyTo(destinationStream);
                }
                destinationStream.Position = 0;
                using (var sr = new StreamReader(destinationStream))
                {
                    var text = await sr.ReadToEndAsync();
                    return text;
                }
            }
        }

    }
}