/**************************************************************************
 * Filename: Program.cs
 * Project: PingTrigger.exe
 * 
 * Description:
 * Entry point for the PingTrigger application.  Configures logging 
 * and launches the service or command line application using Topshelf
**************************************************************************/
using System.IO;
using System.Reflection;
using System.Configuration;
using Serilog;
using Topshelf;
using Ninject;
using System.Net;


namespace PingTrigger
{
    class Program
    {
        static void Main(string[] args)
        {
            // configure the logging system
            Log.Logger = new LoggerConfiguration().CreateLogger();
            Serilog.Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.WithProperty("appname", Assembly.GetEntryAssembly().GetName().Name)
                .WriteTo.ColoredConsole(Serilog.Events.LogEventLevel.Verbose)
                .WriteTo.RollingFile(Path.Combine(".", "serilog-{Date}.txt"), Serilog.Events.LogEventLevel.Debug)
                .CreateLogger();

            // log the program launch and version information
            AssemblyName assemblyName = Assembly.GetEntryAssembly().GetName();
            Log.Information(assemblyName.Name + " Version " + assemblyName.Version.ToString());
            Log.Information("Copyright (c) 2016 - Sanctuary Software Studio, Inc.");

            HostFactory.Run(x =>
            {
                x.Service<PingTriggerService>(s =>
                {
                    s.ConstructUsing(name => new PingTriggerService());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();

                x.SetDescription("PingTrigger Service");
                x.SetDisplayName("PingTrigger");
                x.SetServiceName("PingTrigger");
            });

        }
    }
}
