using Serilog;
using System.Reflection;

using Controlador_4._3_Service;

IHost host = Host.CreateDefaultBuilder(args)

    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
    })
    .UseSerilog((hostingContext, services, loggerConfiguration) => loggerConfiguration
    .ReadFrom.Configuration(hostingContext.Configuration))
    .UseWindowsService()
    .Build();

await host.RunAsync();
