using gatitosEtl.Data.interfaces;
using gatitosEtl.Models.DIMS;

namespace gatitosEtl.Data.services
{
    public class CiudadService
    {
        private readonly ICiudadRepository _ciudadRepository;

        public CiudadService(ICiudadRepository ciudadRepository)
        {
            _ciudadRepository = ciudadRepository;
        }

        public async Task<List<DimCiudad>> GetAllAsync()
        {
            return await _ciudadRepository.GetAllAsync();
        }

        public async Task<DimCiudad?> GetByIdAsync(int id)
        {
            return await _ciudadRepository.GetByIdAsync(id);
        }

        public async Task<bool> ExistsCiudadAsync(string nombre)
        {
            var ciudades = await _ciudadRepository.FindAsync(c => c.nombre == nombre);
            return ciudades.Any();
        }

        public async Task<DimCiudad> AddCiudadAsync(DimCiudad ciudad)
        {
            return await _ciudadRepository.AddAsync(ciudad);
        }
    }
}
