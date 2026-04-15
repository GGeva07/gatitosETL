using gatitosEtl.Data.interfaces;
using gatitosEtl.Models.DIMS;

namespace gatitosEtl.Data.services
{
    public class FechaService
    {
        private readonly IFechaRepository _fechaRepository;

        public FechaService(IFechaRepository fechaRepository)
        {
            _fechaRepository = fechaRepository;
        }

        public async Task<List<DimFecha>> GetAllAsync()
        {
            return await _fechaRepository.GetAllAsync();
        }

        public async Task<DimFecha?> GetByIdAsync(int id)
        {
            return await _fechaRepository.GetByIdAsync(id);
        }

        public async Task<List<DimFecha>> GetByAnioAsync(int anio)
        {
            return await _fechaRepository.FindAsync(f => f.anio == anio);
        }

        public async Task<List<DimFecha>> GetByMesAsync(int mes)
        {
            return await _fechaRepository.FindAsync(f => f.mes == mes);
        }

        public async Task<List<DimFecha>> GetByRangoAsync(DateTime desde, DateTime hasta)
        {
            return await _fechaRepository.FindAsync(f =>
                (f.anio > desde.Year || (f.anio == desde.Year && f.mes > desde.Month) ||
                 (f.anio == desde.Year && f.mes == desde.Month && f.dia >= desde.Day)) &&
                (f.anio < hasta.Year || (f.anio == hasta.Year && f.mes < hasta.Month) ||
                 (f.anio == hasta.Year && f.mes == hasta.Month && f.dia <= hasta.Day)));
        }

        public async Task<DimFecha> AddFechaAsync(DimFecha fecha)
        {
            return await _fechaRepository.AddAsync(fecha);
        }

        public async Task UpdateFechaAsync(DimFecha fecha)
        {
            await _fechaRepository.UpdateAsync(fecha);
        }

        public async Task DeleteFechaAsync(DimFecha fecha)
        {
            await _fechaRepository.DeleteAsync(fecha);
        }
    }
}
