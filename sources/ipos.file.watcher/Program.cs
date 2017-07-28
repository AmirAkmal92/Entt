using System;
using Bespoke.Sph.Domain;
using Topshelf;
using Bespoke.PosEntt.Ipos.FileWatcher;

namespace Bespoke.PosEntt.FileWatcher
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
                    svc.ConstructUsing(() => new IposPemFileDrop());

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
                config.SetServiceName("Rx IPOS File Watcher");
                config.SetDisplayName("Rx IPOS File Watcher");
                config.SetDescription("IPOS file watcher for EnTT Acceptance Platform");

                config.StartAutomatically();
            });
        }
    }
}
