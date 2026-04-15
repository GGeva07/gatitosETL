using gatitosEtl.Data.interfaces;

namespace WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ICsvReaderService _csvReaderService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;

        public Worker(
            ILogger<Worker> logger,
            ICsvReaderService csvReaderService,
            IServiceProvider serviceProvider,
            IHostApplicationLifetime hostApplicationLifetime)
        {
            _logger = logger;
            _csvReaderService = csvReaderService;
            _serviceProvider = serviceProvider;
            _hostApplicationLifetime = hostApplicationLifetime;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _logger.LogInformation("Worker iniciado - Procesando ETL");

                var csvPath = "data/datos.csv";

                if (!File.Exists(csvPath))
                {
                    _logger.LogWarning($"Archivo CSV no encontrado en: {csvPath}");
                    return;
                }

                _logger.LogInformation($"Leyendo CSV desde: {csvPath}");
                var csvData = await _csvReaderService.ReadCsvAsync(csvPath);
                _logger.LogInformation($"Se leyeron {csvData.Count} registros del CSV");

                _logger.LogInformation("Iniciando procesamiento ETL");
                
                using (var scope = _serviceProvider.CreateScope())
                {
                    var etlService = scope.ServiceProvider.GetRequiredService<IEtlService>();
                    var etlResult = await etlService.ProcessEtlAsync(csvData);

                    if (etlResult.Success)
                    {
                        _logger.LogInformation(
                            $"ETL completado exitosamente: " +
                            $"Ciudades={etlResult.CiudadesProcessadas}, " +
                            $"Personas={etlResult.PersonasProcessadas}, " +
                            $"Gatos={etlResult.GatosProcessados}, " +
                            $"Fechas={etlResult.FechasProcessadas}");
                    }
                    else
                    {
                        _logger.LogError($"Error en ETL: {etlResult.Mensaje}");
                        foreach (var error in etlResult.Errores)
                        {
                            _logger.LogError(error);
                        }
                    }
                }

                _hostApplicationLifetime.StopApplication();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante la ejecución del Worker");
                _hostApplicationLifetime.StopApplication();
            }
        }
    }
}
