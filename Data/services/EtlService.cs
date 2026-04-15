using gatitosEtl.Data.interfaces;
using gatitosEtl.Models.DIMS;
using gatitosEtl.Models.DTOs;

namespace gatitosEtl.Data.services
{
    public class EtlService : IEtlService
    {
        private readonly ICiudadRepository _ciudadRepository;
        private readonly IPersonaRepository _personaRepository;
        private readonly IGatoRepository _gatoRepository;
        private readonly IFechaRepository _fechaRepository;

        public EtlService(
            ICiudadRepository ciudadRepository,
            IPersonaRepository personaRepository,
            IGatoRepository gatoRepository,
            IFechaRepository fechaRepository)
        {
            _ciudadRepository = ciudadRepository;
            _personaRepository = personaRepository;
            _gatoRepository = gatoRepository;
            _fechaRepository = fechaRepository;
        }

        public async Task<EtlResultDto> ProcessEtlAsync(List<CsvDataDto> csvData)
        {
            var result = new EtlResultDto();

            try
            {
                result.CiudadesProcessadas = await ProcessCiudadesAsync(csvData);

                result.PersonasProcessadas = await ProcessPersonasAsync(csvData);

                result.GatosProcessados = await ProcessGatosAsync(csvData);

                result.FechasProcessadas = await ProcessFechasAsync(csvData);

                result.Success = true;
                result.Mensaje = "ETL procesado exitosamente";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Mensaje = $"Error durante el procesamiento ETL: {ex.Message}";
                result.Errores.Add(ex.ToString());
            }

            return result;
        }

        private async Task<int> ProcessCiudadesAsync(List<CsvDataDto> csvData)
        {
            var ciudadesUnicas = csvData
                .Where(x => !string.IsNullOrEmpty(x.Ciudad))
                .GroupBy(x => x.Ciudad)
                .Select(g => g.First())
                .ToList();

            int procesadas = 0;

            foreach (var ciudad in ciudadesUnicas)
            {
                try
                {
                    var ciudadExistente = await _ciudadRepository.FindAsync(c => c.nombre == ciudad.Ciudad);
                    
                    if (!ciudadExistente.Any())
                    {
                        var dimCiudad = new DimCiudad
                        {
                            nombre = ciudad.Ciudad!
                        };

                        await _ciudadRepository.AddAsync(dimCiudad);
                        procesadas++;
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Error procesando ciudad '{ciudad.Ciudad}': {ex.Message}", ex);
                }
            }

            return procesadas;
        }

        private async Task<int> ProcessPersonasAsync(List<CsvDataDto> csvData)
        {
            var personasUnicas = csvData
                .Where(x => !string.IsNullOrEmpty(x.Nombre) && !string.IsNullOrEmpty(x.IdPersona))
                .GroupBy(x => x.IdPersona)
                .Select(g => g.First())
                .ToList();

            int procesadas = 0;

            foreach (var persona in personasUnicas)
            {
                try
                {
                    if (!int.TryParse(persona.IdPersona, out var idPersona))
                        throw new InvalidOperationException($"Id de persona inválido: {persona.IdPersona}");

                    var personaExistente = await _personaRepository.GetByIdAsync(idPersona);

                    if (personaExistente == null)
                    {
                        var ciudades = await _ciudadRepository.FindAsync(c => c.nombre == persona.Ciudad);
                        var idCiudad = ciudades.FirstOrDefault()?.id_ciudad ?? 1;

                        var dimPersona = new DimPersona
                        {
                            id_persona = idPersona,
                            nombre = persona.Nombre!,
                            idCiudad = idCiudad
                        };

                        await _personaRepository.AddAsync(dimPersona);
                        procesadas++;
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Error procesando persona '{persona.Nombre}': {ex.Message}", ex);
                }
            }

            return procesadas;
        }

        private async Task<int> ProcessGatosAsync(List<CsvDataDto> csvData)
        {
            var gatosUnicos = csvData
                .Where(x => !string.IsNullOrEmpty(x.IdGato) && !string.IsNullOrEmpty(x.NombreGato))
                .GroupBy(x => x.IdGato)
                .Select(g => g.First())
                .ToList();

            int procesados = 0;

            foreach (var gato in gatosUnicos)
            {
                try
                {
                    if (!int.TryParse(gato.IdGato, out var idGato))
                        throw new InvalidOperationException($"Id de gato inválido: {gato.IdGato}");

                    var gatoExistente = await _gatoRepository.GetByIdAsync(idGato);

                    if (gatoExistente == null)
                    {
                        var dimGato = new DimGato
                        {
                            id_gato = idGato,
                            nombre = gato.NombreGato!,
                            raza = gato.Raza ?? "Desconocida",
                            tipo = gato.Tipo ?? "Desconocido"
                        };

                        await _gatoRepository.AddAsync(dimGato);
                        procesados++;
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Error procesando gato '{gato.NombreGato}': {ex.Message}", ex);
                }
            }

            return procesados;
        }

        private async Task<int> ProcessFechasAsync(List<CsvDataDto> csvData)
        {
            var fechasUnicas = csvData
                .Where(x => !string.IsNullOrEmpty(x.Fecha))
                .GroupBy(x => x.Fecha)
                .Select(g => g.First())
                .ToList();

            int procesadas = 0;

            foreach (var fechaStr in fechasUnicas)
            {
                try
                {
                    if (!DateTime.TryParse(fechaStr.Fecha, out var fecha))
                        throw new InvalidOperationException($"Fecha inválida: {fechaStr.Fecha}");

                    var fechaExistente = await _fechaRepository.FindAsync(f => 
                        f.dia == fecha.Day && 
                        f.mes == fecha.Month && 
                        f.anio == fecha.Year);

                    if (!fechaExistente.Any())
                    {
                        var dimFecha = new DimFecha
                        {
                            dia = fecha.Day,
                            mes = fecha.Month,
                            anio = fecha.Year
                        };

                        await _fechaRepository.AddAsync(dimFecha);
                        procesadas++;
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Error procesando fecha '{fechaStr.Fecha}': {ex.Message}", ex);
                }
            }

            return procesadas;
        }
    }
}
