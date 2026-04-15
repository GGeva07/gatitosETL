using gatitosEtl.Data.interfaces;

namespace WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ICsvReaderService _csvReaderService;
        private readonly IEtlService _etlService;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;

        public Worker(
            ILogger<Worker> logger,
            ICsvReaderService csvReaderService,
            IEtlService etlService,
            IHostApplicationLifetime hostApplicationLifetime)
        {
            _logger = logger;
            _csvReaderService = csvReaderService;
            _etlService = etlService;
            _hostApplicationLifetime = hostApplicationLifetime;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _logger.LogInformation("Worker iniciado - Procesando ETL");

                // Ruta del archivo CSV (puede configurarse en appsettings.json)
                var csvPath = "data/datos.csv";

                if (!File.Exists(csvPath))
                {
                    _logger.LogWarning($"Archivo CSV no encontrado en: {csvPath}");
                    return;
                }

                // Leer datos del CSV
                _logger.LogInformation($"Leyendo CSV desde: {csvPath}");
                var csvData = await _csvReaderService.ReadCsvAsync(csvPath);
                _logger.LogInformation($"Se leyeron {csvData.Count} registros del CSV");

                // Procesar ETL
                _logger.LogInformation("Iniciando procesamiento ETL");
                var etlResult = await _etlService.ProcessEtlAsync(csvData);

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

                // Detener el servicio después de completar
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
