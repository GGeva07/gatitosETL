using gatitosEtl.Data.interfaces;
using gatitosEtl.Models.DIMS;

namespace gatitosEtl.Data.services
{
    public class GatoService
    {
        private readonly IGatoRepository _gatoRepository;

        public GatoService(IGatoRepository gatoRepository)
        {
            _gatoRepository = gatoRepository;
        }

        public async Task<List<DimGato>> GetAllAsync()
        {
            return await _gatoRepository.GetAllAsync();
        }

        public async Task<DimGato?> GetByIdAsync(int id)
        {
            return await _gatoRepository.GetByIdAsync(id);
        }

        public async Task<List<DimGato>> GetByNombreAsync(string nombre)
        {
            return await _gatoRepository.FindAsync(g => g.nombre.Contains(nombre));
        }

        public async Task<List<DimGato>> GetByRazaAsync(string raza)
        {
            return await _gatoRepository.FindAsync(g => g.raza == raza);
        }

        public async Task<List<DimGato>> GetByTipoAsync(string tipo)
        {
            return await _gatoRepository.FindAsync(g => g.tipo == tipo);
        }

        public async Task<DimGato> AddGatoAsync(DimGato gato)
        {
            return await _gatoRepository.AddAsync(gato);
        }

        public async Task UpdateGatoAsync(DimGato gato)
        {
            await _gatoRepository.UpdateAsync(gato);
        }

        public async Task DeleteGatoAsync(DimGato gato)
        {
            await _gatoRepository.DeleteAsync(gato);
        }
    }
}
