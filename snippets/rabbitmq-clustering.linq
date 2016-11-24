<Query Kind="Program">
  <Reference Relative="..\subscribers\domain.sph.dll">F:\project\work\entt.rts\subscribers\domain.sph.dll</Reference>
  <NuGetReference Version="3.6.0">RabbitMQ.Client</NuGetReference>
  <Namespace>Bespoke.Sph.Domain</Namespace>
  <Namespace>RabbitMQ.Client</Namespace>
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

void Main()
{
	var host = "S323";
	var factory = new ConnectionFactory
	{
		UserName = ConfigurationManager.RabbitMqUserName,
		Password = ConfigurationManager.RabbitMqPassword,
		HostName = host,
		Port = ConfigurationManager.RabbitMqPort,
		VirtualHost = ConfigurationManager.RabbitMqVirtualHost
	};

	var connection = factory.CreateConnection();
	var channel = connection.CreateModel();
	Console.WriteLine($"Connecting to host {factory.HostName}");
	connection.ConnectionShutdown += (sender, args) =>
	{
		Console.WriteLine("Broker shutdown");
		args.Dump();
		// dispose previous connections
		// channel.Dispose();
		// connection.Dispose();

		host = (host == "S322") ? "S323" : "S322";
		factory = new ConnectionFactory
		{
			UserName = ConfigurationManager.RabbitMqUserName,
			Password = ConfigurationManager.RabbitMqPassword,
			HostName = host,
			Port = ConfigurationManager.RabbitMqPort,
			VirtualHost = ConfigurationManager.RabbitMqVirtualHost
		};
		connection = factory.CreateConnection();
		channel = connection.CreateModel();
		Console.WriteLine($"Connecting to host {factory.HostName}");

	};

	const string ROUTING_KEY = "test2";
	foreach (var k in connection.ServerProperties.Keys)
	{
		var content = connection.ServerProperties[k] as byte[];
		if (null == content) continue;
		using (var orginalStream = new MemoryStream(content))
		{
			using (var sr = new StreamReader(orginalStream))
			{
				var text = sr.ReadToEnd();
				Console.WriteLine($"{k} = {text}");
			}
		}
	}

	Console.WriteLine("Give session a name");
	var session = Console.ReadLine();
	Console.WriteLine("Enter to send another message, 'q' to quit");
	var quit = "";
	var count = 0;
	while (quit != "q")
	{
		while (!connection.IsOpen)
		{
			Thread.Sleep(100);
		}
		var message = $"{session} # {count++}";
		Console.WriteLine("Sending " + message);
		var body = Encoding.UTF8.GetBytes(message);
		var props = channel.CreateBasicProperties();
		props.DeliveryMode = 2;
		props.ContentType = "application/json";
		props.Headers = new Dictionary<string, object>();


		channel.BasicPublish("sph.topic", ROUTING_KEY, props, body);

		var rs = new AutoResetEvent(false);
		ThreadPool.QueueUserWorkItem(_ =>
		{
			quit = Console.ReadLine();
			rs.Set();
		}, new object());
		rs.WaitOne(1000);
	}


	Thread.Sleep(500);
	channel.Close();
	connection.Close();
	channel.Dispose();
	connection.Dispose();
}

// Define other methods and classes here
public class HostnameSelector : IHostnameSelector
{
	public string NextFrom(IList<string> lists)
	{
		Console.WriteLine(lists);
		return lists[0];
	}
}