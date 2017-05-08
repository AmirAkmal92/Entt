using System;
using Bespoke.Sph.Domain;
using Topshelf;

namespace Bespoke.PosEntt.ReceiveLocations
{
    public class Program
    {

        public static void Main(string[] args)
        {
            HostFactory.Run(config =>
            {
                config.UseNLog();
                config.Service<IReceiveLocation>(svc =>
                {
                    svc.ConstructUsing(() => new VanScanSqlReceiveLocation());
                

                    svc.WhenStarted(x => x.Start());
                    svc.WhenStopped(x => x.Stop());
                    svc.WhenPaused(x => x.Pause());
                    svc.WhenContinued(x => x.Resume());
                    svc.WhenShutdown(x => x.Stop());
                    svc.WhenCustomCommandReceived((x, g, i) =>
                    {
                        Console.WriteLine(g);
                        Console.WriteLine(i);
                    });
                });

                var instanceName = args.ParseArg("v");
                if (string.IsNullOrWhiteSpace(instanceName))
                    throw new ArgumentException("Please provide /v:<instance-name> when installing the service", nameof(args));

                config.SetServiceName($"RxSqlDropLocation{instanceName}");
                config.SetDisplayName($"Rx Receive Location {instanceName}");
                config.SetDescription($"Rx Developer receive location for {instanceName}");

                config.StartAutomatically();
            });

        }

  


    }
}
