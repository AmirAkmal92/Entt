using System;
using System.Linq;

namespace Bespoke.PosEntt.ReceiveLocations
{
    public static class CommandLineArgumentExtensions
    {
        public static string ParseArg(this string[] args, string name)
        {
            args = args.Any() ? args : Environment.CommandLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var val = args.SingleOrDefault(a => a.StartsWith("/" + name + ":"));
            return val?.Replace("/" + name + ":", string.Empty);
        }
    }
}