using Serilog;
using System.Diagnostics;
using System.Management;
using System.ServiceProcess;
using System.Timers;

namespace Controlador_4._3_Service
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;
        ControladorConfig serviceConfig;

        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;

            serviceConfig = _configuration.GetSection("ServiceConfig").Get<ControladorConfig>();

            Log.Information("Iniciando servico");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            Log.Debug($"MQ Controlador 4.3 Start");

            List<string> services = new List<string>();

            System.Timers.Timer timer = new System.Timers.Timer();

            // calcula horario
            DateTime agora = DateTime.Now;
            DateTime horaAlvoData = new DateTime(agora.Year, agora.Month, agora.Day, serviceConfig.StartHour, serviceConfig.StartMinute, 0);
            TimeSpan tempoRestante = horaAlvoData - agora;

            // verifica se passou do horario, adiciona um dia
            if (tempoRestante.TotalMilliseconds < 0)
            {
                tempoRestante = tempoRestante.Add(TimeSpan.FromDays(1));
            }

            // Configure o Timer para disparar o evento após o tempo restante.
            //timer.Interval = tempoRestante.TotalMilliseconds;
            //timer.Elapsed += (sender, e) =>
            //{
            //    timer.Stop();
            services = GetServiceList(serviceConfig.ServiceListPath);
            RestartServices(services);
            //    // configura proxima execucao
            //    timer.Interval = TimeSpan.FromDays(1).TotalMilliseconds;
            //    timer.Start();
            //};

            //timer.Start();

            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            Log.Debug($"MQ Controlador 4.3 Start");

            return base.StopAsync(cancellationToken);
        }

        private List<String> GetServiceList(string filePath)
        {
            List<String> serviceList = new List<String>();

            if (File.Exists(filePath))
            {
                serviceList = File.ReadLines(filePath).ToList();
            }            

            return serviceList;
        }
               
        private void RestartServices(List<string> services)
        {
            StopService(services);
            StartService(services);
        }

        public static void StopService(List<string> servicesName)
        {
            Log.Information("Stopping");
            //Parallel.ForEach( servicesName, service => {
            //    StopService(service);
            //});            
            foreach (var service in servicesName)
            {
                StopService(service);
            }
        }

        public static void StopService(string serviceName)
        {
            using (ServiceController serviceController = new ServiceController(serviceName))
            {
                try
                {
                    if (serviceController.Status == ServiceControllerStatus.Running)
                    {
                        serviceController.Stop();
                        serviceController.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(10));
                        Log.Information($"{serviceName} parado com sucesso.");
                    }
                    else
                    {
                        Log.Information($"{serviceName} já está parado.");
                    }
                }
                catch (Exception ex)
                {
                    Log.Error($"Erro ao parar o serviço {serviceName}: {ex.Message} \r\n {ex.StackTrace}" );
                    KillProcess(serviceName);
                }
            }
        }

        private static void KillProcess(string serviceName)
        {
            try
            {
                int processId = GetProcessIdByServiceName(serviceName);

                var process = Process.GetProcessById(processId);
                process.Kill();
            }
            catch (Exception ex)
            {
                Log.Error($"");
            }
        }

        private static int GetProcessIdByServiceName(string serviceName)
        {

            string qry = $"SELECT PROCESSID FROM WIN32_SERVICE WHERE NAME = '{serviceName}'";
            var searcher = new ManagementObjectSearcher(qry);
            var managementObjects = new ManagementObjectSearcher(qry).Get();

            if (managementObjects.Count != 1)
                throw new InvalidOperationException($"In attempt to kill a service '{serviceName}', expected to find one process for service but found {managementObjects.Count}.");

            int processId = 0;

            foreach (ManagementObject mngntObj in managementObjects)
                processId = (int)(uint)mngntObj["PROCESSID"];

            if (processId == 0)
                throw new InvalidOperationException($"In attempt to kill a service '{serviceName}', process ID for service is 0. Possible reason is the service is already stopped.");

            return processId;
        }

        public static void StartService(List<string> servicesName)
        {
            Log.Information("Starting");
            Parallel.ForEach(servicesName, service => {
                StartService(service);
            });            
        }

        public static void StartService(string serviceName)
        {
            using (ServiceController serviceController = new ServiceController(serviceName))
            {
                try
                {
                    if (serviceController.Status == ServiceControllerStatus.Stopped)
                    {
                        serviceController.Start();
                        serviceController.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(30));
                        Log.Information($"{serviceName} iniciado com sucesso.");
                    }
                    else
                    {
                        Log.Information($"{serviceName} já está iniciado.");
                    }
                }
                catch (Exception ex)
                {
                    Log.Error($"Erro ao iniciar o serviço {serviceName}: {ex.Message} \r\n {ex.StackTrace}");
                }
            }
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