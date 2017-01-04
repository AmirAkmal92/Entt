<Query Kind="Program">
  <Reference Relative="..\subscribers\domain.sph.dll">F:\project\work\entt.rts\subscribers\domain.sph.dll</Reference>
  <Reference Relative="..\subscribers.host\subscriber.infrastructure.dll">F:\project\work\entt.rts\subscribers.host\subscriber.infrastructure.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Net.Http.dll</Reference>
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <NuGetReference Version="3.6.0">RabbitMQ.Client</NuGetReference>
  <Namespace>Bespoke.Sph.Domain</Namespace>
  <Namespace>Bespoke.Sph.SubscribersInfrastructure</Namespace>
  <Namespace>Newtonsoft.Json.Linq</Namespace>
  <Namespace>RabbitMQ.Client</Namespace>
  <Namespace>System.Collections.Concurrent</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <AppConfig>
    <Content>
      <configuration>
        <appSettings>
          <add key="sph:ApplicationName" value="PosEntt" />
        </appSettings>
      </configuration>
    </Content>
  </AppConfig>
</Query>

static void Main()
{
	var host = "S322";
	var factory = new ConnectionFactory
	{
		UserName = ConfigurationManager.RabbitMqUserName,
		Password = ConfigurationManager.RabbitMqPassword,
		HostName = host,
		Port = ConfigurationManager.RabbitMqPort,
		VirtualHost = ConfigurationManager.RabbitMqVirtualHost
	};
	var connection = factory.CreateConnection();

	var flag = new AutoResetEvent(false);
	Console.WriteLine($"Connecting to host {factory.HostName}");
	connection.ConnectionShutdown += (sender, args) =>
	{
		Console.WriteLine("Broker shutdown");
		args.Dump();
		flag.Set();

	};

	var startupCount = 3;
	var maxIntances = 50;
	var minInstances = 2;

	var subscribers = new ConcurrentBag<Subscriber>();
	for (int i = 0; i < startupCount; i++)
	{
		var sub = new Subscriber { InstanceName = (i + 1).ToString() };
		sub.Start(connection);
		subscribers.Add(sub);
	}


	var handler = new HttpClientHandler { Credentials = new NetworkCredential(ConfigurationManager.RabbitMqUserName, ConfigurationManager.RabbitMqPassword) };
	var client = new HttpClient(handler) { BaseAddress = new Uri("http://s322:15672") };
	Console.WriteLine("'q' to quit or 'r' to reconnect");
	var quit = "";
	while (quit != "q")
	{
		quit = Console.ReadLine();
		if (quit == "q")
		{
			ThreadPool.QueueUserWorkItem(_ =>
			{
				Thread.Sleep(500);
				flag.Set();
			});
		}

		if (quit != "q")
		{
			// read the publish rate and delivery rate
			var response = client.GetStringAsync("api/queues/PosEntt/test2")
			.ContinueWith(_ =>
			{

				var json = JObject.Parse(_.Result);
				var published = json.SelectToken("$.message_stats.publish_details.rate").Value<double>();
				var delivered = json.SelectToken("$.message_stats.deliver_details.rate").Value<double>();
				var length =  json.SelectToken("$.messages").Value<double>();
				
				var processing = subscribers.Sum(x => x.PrefetchCount);
				var overloaded = ( published > delivered + processing ) || (length > processing);

				Console.WriteLine($"Published:{published}, Delivered : {delivered}, length : {length} , Processing {processing}");
				if (overloaded && subscribers.Count < maxIntances)
				{
					var sub1 = new Subscriber { InstanceName = (subscribers.Count + 1 ).ToString() };
					sub1.Start(connection);
					subscribers.Add(sub1);
				}

				if (!overloaded && subscribers.Count > minInstances)
				{
					Subscriber sub2;
					if (subscribers.TryTake(out sub2))
					{
						sub2.Stop();
					}
				}
				Console.WriteLine("Current subscribers " + subscribers.Count);
			});
		}

	}
	flag.WaitOne(1500);

	if (connection.IsOpen) connection.Close();
	connection.Dispose();
}

// Define other methods and classes here
public class Subscriber
{
	private TaskCompletionSource<bool> m_stoppingTcs;
	private IModel m_channel;
	private TaskBasicConsumer m_consumer;
	private int m_processing;
	public string InstanceName { get; set; }
	public string QueueName { get; set; } = "test2";
	const bool NO_ACK = false;
	public int PrefetchCount { get; set; } = 1;

	public void Start(IConnection connection)
	{
		Console.WriteLine("Starting : {0} : {1}", this.InstanceName, this.QueueName);

		m_stoppingTcs = new TaskCompletionSource<bool>();
		m_channel = connection.CreateModel();

		m_channel.BasicQos(0, (ushort)this.PrefetchCount, false);
		m_consumer = new TaskBasicConsumer(m_channel);
		m_consumer.Received += Received;
		m_channel.BasicConsume(this.QueueName, NO_ACK, m_consumer);
		Console.WriteLine("Started : {0} : {1} !!!", this.InstanceName, this.QueueName);
	}

	public async void Received(object sender, Bespoke.Sph.SubscribersInfrastructure.ReceivedMessageArgs e)
	{
		Interlocked.Increment(ref m_processing);
		try
		{
			var message = Encoding.UTF8.GetString(e.Body);
			await this.ProcessMessageAsync(message);
			m_channel.BasicAck(e.DeliveryTag, false);

		}
		finally
		{
			Interlocked.Decrement(ref m_processing);
		}

	}

	private async Task ProcessMessageAsync(string message)
	{
		var log = $"Receiving {message} at {DateTime.Now:HH:mm:ss} on {InstanceName}";
		await Task.Delay(250);
		Console.WriteLine($"{log} finished {DateTime.Now:HH:mm:ss}");

	}

	public void Stop()
	{
		Console.WriteLine("!!Stoping : {0} : {1}", this.InstanceName, this.QueueName);

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

		Console.WriteLine("Stopped : {0} : {1}", this.InstanceName, this.QueueName);
	}

}