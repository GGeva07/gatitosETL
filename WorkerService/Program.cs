using gatitosEtl.Data.context;
using gatitosEtl.Data.interfaces;
using gatitosEtl.Data.repo;
using gatitosEtl.Data.services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WorkerService;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        var connStr = context.Configuration["ConnectionStrings:DWH"]
                      ?? throw new InvalidOperationException(
                             "Falta 'ConnectionStrings:DWH' en appsettings.json");

        services.AddDbContext<DbGatitosContext>(options =>
            options.UseSqlServer(connStr));

        services.AddScoped<ICiudadRepository, CiudadRepository>();
        services.AddScoped<IPersonaRepository, PersonaRepository>();
        services.AddScoped<IGatoRepository, GatoRepository>();
        services.AddScoped<IFechaRepository, FechaRepository>();

        services.AddScoped<ICsvReaderService, CsvReaderService>();
        services.AddScoped<IEtlService, EtlService>();

        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();