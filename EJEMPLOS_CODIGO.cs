// ============================================================================
// EJEMPLOS DE USO - Sistema ETL gatitosETL
// ============================================================================

using gatitosEtl.Data.interfaces;
using gatitosEtl.Data.services;
using gatitosEtl.Models.DIMS;

// ============================================================================
// 1. USO BÁSICO DEL SERVICIO DE LECTURA CSV
// ============================================================================

public class CsvReaderExample
{
    private readonly ICsvReaderService _csvReaderService;

    public async Task LeerCsvAsync()
    {
        // Leer archivo CSV
        var datos = await _csvReaderService.ReadCsvAsync("data/datos.csv");
        
        // Procesar datos
        foreach (var registro in datos)
        {
            Console.WriteLine($"Persona: {registro.Nombre}, Ciudad: {registro.Ciudad}");
        }
    }
}

// ============================================================================
// 2. USO DEL SERVICIO ETL
// ============================================================================

public class EtlExample
{
    private readonly ICsvReaderService _csvReaderService;
    private readonly IEtlService _etlService;

    public async Task ProcesarEtlAsync()
    {
        // Paso 1: Leer CSV
        var csvData = await _csvReaderService.ReadCsvAsync("data/datos.csv");
        
        // Paso 2: Procesar ETL
        var resultado = await _etlService.ProcessEtlAsync(csvData);
        
        // Paso 3: Verificar resultado
        if (resultado.Success)
        {
            Console.WriteLine("? ETL completado");
            Console.WriteLine($"  Ciudades: {resultado.CiudadesProcessadas}");
            Console.WriteLine($"  Personas: {resultado.PersonasProcessadas}");
            Console.WriteLine($"  Gatos: {resultado.GatosProcessados}");
            Console.WriteLine($"  Fechas: {resultado.FechasProcessadas}");
        }
        else
        {
            Console.WriteLine("? Error en ETL:");
            foreach (var error in resultado.Errores)
            {
                Console.WriteLine($"  - {error}");
            }
        }
    }
}

// ============================================================================
// 3. OPERACIONES CON CIUDADES
// ============================================================================

public class CiudadExample
{
    private readonly CiudadService _ciudadService;

    public async Task EjemploCiudadAsync()
    {
        // Obtener todas las ciudades
        var ciudades = await _ciudadService.GetAllAsync();
        Console.WriteLine($"Total ciudades: {ciudades.Count}");

        // Agregar nueva ciudad
        var nuevaCiudad = new DimCiudad { nombre = "Valencia" };
        await _ciudadService.AddCiudadAsync(nuevaCiudad);

        // Verificar si existe
        bool existe = await _ciudadService.ExistsCiudadAsync("Madrid");
        Console.WriteLine($"¿Madrid existe? {existe}");

        // Obtener por ID
        var ciudad = await _ciudadService.GetByIdAsync(1);
        if (ciudad != null)
        {
            Console.WriteLine($"Ciudad: {ciudad.nombre}");
        }
    }
}

// ============================================================================
// 4. OPERACIONES CON PERSONAS
// ============================================================================

public class PersonaExample
{
    private readonly PersonaService _personaService;

    public async Task EjemploPersonaAsync()
    {
        // Obtener todas
        var personas = await _personaService.GetAllAsync();
        
        // Obtener por nombre
        var juanes = await _personaService.GetByNombreAsync("Juan");
        
        // Obtener personas de una ciudad (idCiudad = 1)
        var personasMadrid = await _personaService.GetByCiudadAsync(1);
        
        // Agregar persona
        var nuevaPersona = new DimPersona 
        { 
            id_persona = 10,
            nombre = "Laura González", 
            idCiudad = 1 
        };
        await _personaService.AddPersonaAsync(nuevaPersona);
        
        // Actualizar
        nuevaPersona.nombre = "Laura María González";
        await _personaService.UpdatePersonaAsync(nuevaPersona);
        
        // Eliminar
        await _personaService.DeletePersonaAsync(nuevaPersona);
    }
}

// ============================================================================
// 5. OPERACIONES CON GATOS
// ============================================================================

public class GatoExample
{
    private readonly GatoService _gatoService;

    public async Task EjemploGatoAsync()
    {
        // Obtener todos los gatos
        var gatos = await _gatoService.GetAllAsync();
        
        // Buscar gatos de una raza
        var persas = await _gatoService.GetByRazaAsync("Persa");
        Console.WriteLine($"Gatos Persas: {persas.Count}");
        
        // Buscar gatos de un tipo
        var domesticos = await _gatoService.GetByTipoAsync("Doméstico");
        
        // Agregar gato
        var nuevoGato = new DimGato
        {
            id_gato = 200,
            nombre = "Luna",
            raza = "Siamés",
            tipo = "Doméstico"
        };
        await _gatoService.AddGatoAsync(nuevoGato);
    }
}

// ============================================================================
// 6. OPERACIONES CON FECHAS
// ============================================================================

public class FechaExample
{
    private readonly FechaService _fechaService;

    public async Task EjemploFechaAsync()
    {
        // Obtener todas las fechas
        var fechas = await _fechaService.GetAllAsync();
        
        // Obtener fechas de un año
        var fechas2024 = await _fechaService.GetByAnioAsync(2024);
        
        // Obtener fechas de un mes
        var fechasEnero = await _fechaService.GetByMesAsync(1);
        
        // Obtener rango de fechas
        var desde = new DateTime(2024, 1, 1);
        var hasta = new DateTime(2024, 12, 31);
        var rangoFechas = await _fechaService.GetByRangoAsync(desde, hasta);
        
        // Agregar fecha
        var nuevaFecha = new DimFecha
        {
            dia = 25,
            mes = 12,
            anio = 2024
        };
        await _fechaService.AddFechaAsync(nuevaFecha);
    }
}

// ============================================================================
// 7. CONSULTAS COMPLEJAS (usando GenericRepository)
// ============================================================================

public class ConsultasComplejasExample
{
    private readonly IPersonaRepository _personaRepository;
    private readonly ICiudadRepository _ciudadRepository;

    public async Task EjemploConsultasAsync()
    {
        // Contar todas las personas
        int total = await _personaRepository.CountAsync();
        
        // Contar personas de una ciudad
        int personasMadrid = await _personaRepository.CountAsync(p => p.idCiudad == 1);
        
        // Buscar personas con predicado
        var personasConJ = await _personaRepository.FindAsync(p => p.nombre.Contains("J"));
        
        // Verificar si existe una ciudad
        bool existeBarcelona = await _ciudadRepository.ExistsAsync(c => c.nombre == "Barcelona");
        
        // Insertar si no existe
        var madrid = new DimCiudad { nombre = "Madrid" };
        await _ciudadRepository.InsertIfNotExistsAsync(madrid);
    }
}

// ============================================================================
// 8. PROCESAMIENTO DENTRO DEL WORKER SERVICE
// ============================================================================

/*
// En Worker.cs
protected override async Task ExecuteAsync(CancellationToken stoppingToken)
{
    try
    {
        _logger.LogInformation("Worker iniciado");
        
        // Leer CSV
        var csvData = await _csvReaderService.ReadCsvAsync("data/datos.csv");
        _logger.LogInformation($"Leyeron {csvData.Count} registros");
        
        // Procesar ETL
        var resultado = await _etlService.ProcessEtlAsync(csvData);
        
        // Reportar resultados
        if (resultado.Success)
        {
            _logger.LogInformation("? ETL completado exitosamente");
        }
        else
        {
            _logger.LogError("? Error en ETL: {Mensaje}", resultado.Mensaje);
        }
        
        // Detener el servicio
        _hostApplicationLifetime.StopApplication();
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error fatal");
        _hostApplicationLifetime.StopApplication();
    }
}
*/

// ============================================================================
// 9. ASYNC/AWAIT PATTERNS
// ============================================================================

/*
// Patrón 1: Esperar una operación
var persona = await _personaService.GetByIdAsync(1);

// Patrón 2: Esperar múltiples operaciones secuencialmente
var persona = await _personaService.GetByIdAsync(1);
await _personaService.UpdatePersonaAsync(persona);

// Patrón 3: Esperar múltiples operaciones en paralelo
var task1 = _personaService.GetAllAsync();
var task2 = _gatoService.GetAllAsync();
var task3 = _ciudadService.GetAllAsync();
await Task.WhenAll(task1, task2, task3);

var personas = await task1;
var gatos = await task2;
var ciudades = await task3;

// Patrón 4: Try-catch con async
try
{
    await _etlService.ProcessEtlAsync(csvData);
}
catch (Exception ex)
{
    _logger.LogError(ex, "Error procesando ETL");
}
*/

// ============================================================================
// 10. INYECCIÓN DE DEPENDENCIAS
// ============================================================================

/*
// En Program.cs
services.AddDbContext<DbGatitosContext>(options =>
    options.UseSqlServer(connStr));

services.AddScoped<ICiudadRepository, CiudadRepository>();
services.AddScoped<IPersonaRepository, PersonaRepository>();
services.AddScoped<IGatoRepository, GatoRepository>();
services.AddScoped<IFechaRepository, FechaRepository>();

services.AddScoped<CiudadService>();
services.AddScoped<PersonaService>();
services.AddScoped<GatoService>();
services.AddScoped<FechaService>();

services.AddScoped<ICsvReaderService, CsvReaderService>();
services.AddScoped<IEtlService, EtlService>();

services.AddHostedService<Worker>();
*/

// ============================================================================
// FIN DE EJEMPLOS
// ============================================================================
