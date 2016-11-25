<Query Kind="Statements">
  <Reference Relative="..\subscribers\domain.sph.dll">F:\project\work\entt.rts\subscribers\domain.sph.dll</Reference>
  <Reference Relative="..\subscribers.host\subscriber.infrastructure.dll">F:\project\work\entt.rts\subscribers.host\subscriber.infrastructure.dll</Reference>
  <NuGetReference Version="3.6.0">RabbitMQ.Client</NuGetReference>
  <Namespace>Bespoke.Sph.Domain</Namespace>
  <Namespace>RabbitMQ.Client</Namespace>
  <Namespace>Bespoke.Sph.SubscribersInfrastructure</Namespace>
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
var channel = connection.CreateModel();

var flag = new AutoResetEvent(false);
Console.WriteLine($"Connecting to host {factory.HostName}");
connection.ConnectionShutdown += (sender, args) =>
{
	Console.WriteLine("Broker shutdown");
	args.Dump();
	flag.Set();

};

var consumer = new TaskBasicConsumer(channel);
consumer.Received += (sender, e) =>
{
	var message = Encoding.UTF8.GetString(e.Body);
	Console.WriteLine("Receiving " + message);
	channel.BasicAck(e.DeliveryTag, false);
};
channel.BasicConsume("test2", false, consumer);

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
	if (!connection.IsOpen && quit == "r")
	{
		connection = factory.CreateConnection();
		channel = connection.CreateModel();

		consumer = new TaskBasicConsumer(channel);
		consumer.Received += (sender, e) =>
		{
			var message = Encoding.UTF8.GetString(e.Body);
			Console.WriteLine("Receiving " + message);
			channel.BasicAck(e.DeliveryTag, false);
		};
		channel.BasicConsume("test2", false, consumer);
	}
}
flag.WaitOne(1500);

if (channel.IsOpen) channel.Close();
if (connection.IsOpen) connection.Close();
channel.Dispose();
connection.Dispose();