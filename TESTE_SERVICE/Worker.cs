//using Serilog;
using System.ServiceProcess;
using System.Timers;

namespace Controlador_4._3_Service
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;
        //ControladorConfig serviceConfig;

        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;

            //serviceConfig = _configuration.GetSection("ServiceConfig").Get<ControladorConfig>();

            //Log.Logger = new LoggerConfiguration()
            //   .MinimumLevel.Verbose()
            //   .WriteTo.File(serviceConfig.LogPath, rollingInterval: RollingInterval.Day, retainedFileCountLimit: 30)
            //   .CreateLogger();

            //Log.Information("MQ Controlador 4.3 iniciado");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            //Log.Debug($"MQ Controlador 4.3 Start");
            Thread.Sleep( 10000 );
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            //Log.Debug($"MQ Controlador 4.3 Stop");
            Thread.Sleep(10000);
            return base.StopAsync(cancellationToken);
        }

    }

    public class ControladorConfig
    {
        public int StartHour { get; set; }
        public int StartMinute { get; set; }
        public string LogPath { get; set; }
        public string ServiceListPath { get; set; }
    }
}