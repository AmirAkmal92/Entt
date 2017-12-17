using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;

namespace Bespoke.PosEntt.DlqRequeue
{
    internal class Program
    {
        static string queueName = "ms_dead_letter_queue";

        public static async Task Main(string[] args)
        {
            var program = new Program();
            if (args.Length < 5) //If IP is defined but not the count for clearing
            {
                var ip = args[0];
                var user = args[1];
                var pass = args[2];
                await program.RunAsync(ip, null, user, pass);
            }
            else if (args.Length > 4)
            {
                var countClearing = args[0];
                var ip = args[1];
                await program.RunAsync(ip, countClearing, args[2], args[3]);
            }
        }
        private async Task RunAsync(string ip, string countClearing, string user, string pass)
        {
            var factory = new ConnectionFactory();
            var ipC = $"amqp://{user}:{pass}@{ip}:5672/PosEntt";
            factory.Uri = ipC;

            var conn = factory.CreateConnection();
            await Task.Delay(1000);
            var channel = conn.CreateModel();


            await Task.Delay(1000);

            var result = channel.BasicGet(queueName, false);
            var props = result.BasicProperties;
            while (result != null)
            {
                Console.WriteLine("Current Message Count : " + channel.MessageCount(queueName));

                props.DeliveryMode = 2;
                props.ContentType = "application/json";
                props.Headers = result.BasicProperties.Headers;
                props = result.BasicProperties;

                try
                {
                    var body = result.Body;
                    var messageTextBody = await DecompressAsync(body);
                    // 1205, 2601(skip)
                    (var exceptionType, var exceptionMessage, var sqlErrorNumbers) = await GetErrorInfoAsync(messageTextBody);
                    if (sqlErrorNumbers.Contains(2601))
                    {
                        // further processing
                        File.WriteAllText("patht.log", messageTextBody);
                        continue;
                    }
                    //TODO : for any other error type and message
                    Console.WriteLine($"Inspect {exceptionType} : {exceptionMessage} rules");




                    // requeue
                    channel.BasicPublish("sph.topic", result.RoutingKey, props, result.Body);
                    channel.BasicAck(result.DeliveryTag, false);

                    await Task.Delay(150);
                }

                catch (Exception exc)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error processing result");
                    Console.WriteLine($"{exc.GetType().FullName} : {exc.Message}");

                }
                finally
                {
                    Console.ResetColor();
                    result = channel.BasicGet(queueName, false);
                }
            }
        }

        private readonly HttpClient m_client;
        public Program()
        {
            m_client = new HttpClient
            {
                BaseAddress = new Uri(Environment.GetEnvironmentVariable("RX_POSENTT_ElasticsearchHost")
                ?? "http://localhost:9200")
            };
        }

        private async Task<(string ExceptionType, string ErrorMessage, int[] SqlErrorNumbers)> GetErrorInfoAsync(string message)
        {
            var jo = JObject.Parse(message);
            var idToken = jo.SelectToken("$.Id") ?? jo.SelectToken("$.attached[0].Id");

            var id = idToken?.Value<string>();
            if (string.IsNullOrWhiteSpace(id)) return ("", "", Array.Empty<int>());
            id = id.Replace("-", "");
            var request = $@"{{
   ""filter"": {{
      ""query"": {{
         ""bool"": {{
            ""must"": [
               {{
                  ""term"": {{
                     ""otherInfo.id2"": ""{id}""
                  }}
               }}
            ]
         }}
      }}
   }}
}}";
            var rc = new StringContent(request);
            var response = await m_client.PostAsync("/posentt_logs/log/_search", rc);


            if (!(response.Content is StreamContent content)) throw new Exception("Cannot execute query on es " + request);
            var responseText = await content.ReadAsStringAsync();
            var responseJson = JObject.Parse(responseText);
            var total = responseJson.SelectToken("$.hits.total").Value<int>();
            Console.WriteLine(total);
            if (total != 1) return ("", "", Array.Empty<int>());

            var hits = responseJson.SelectToken("$.hits.hits");
            foreach (var hit in hits)
            {
                var src = hit.SelectToken("$._source");
                var exceptionType = src.SelectToken("$.exception.ClassName").Value<string>();
                var exceptionMessage = src.SelectToken("$.exception.Message").Value<string>();
                if (src.SelectToken("$.exception.Errors") is JArray errors)
                {
                    var numbers = errors.Select(er => er.SelectToken("$.number").Value<int>()).ToArray();
                    return (exceptionType, exceptionMessage, numbers);
                }
                return (exceptionType, exceptionMessage, Array.Empty<int>());
            }

            return ("", "", Array.Empty<int>());

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
                    var json = await sr.ReadToEndAsync();
                    return json;

                }
            }

        }

        public static string Message { get; set; }

    }
}
