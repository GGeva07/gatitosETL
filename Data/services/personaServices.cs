using gatitosEtl.Data.interfaces;
using gatitosEtl.Models.DIMS;

namespace gatitosEtl.Data.services
{
    public class PersonaService
    {
        private readonly IPersonaRepository _personaRepository;

        public PersonaService(IPersonaRepository personaRepository)
        {
            _personaRepository = personaRepository;
        }

        public async Task<List<DimPersona>> GetAllAsync()
        {
            return await _personaRepository.GetAllAsync();
        }

        public async Task<DimPersona?> GetByIdAsync(int id)
        {
            return await _personaRepository.GetByIdAsync(id);
        }

        public async Task<List<DimPersona>> GetByNombreAsync(string nombre)
        {
            return await _personaRepository.FindAsync(p => p.nombre.Contains(nombre));
        }

        public async Task<List<DimPersona>> GetByCiudadAsync(int idCiudad)
        {
            return await _personaRepository.FindAsync(p => p.idCiudad == idCiudad);
        }

        public async Task<DimPersona> AddPersonaAsync(DimPersona persona)
        {
            return await _personaRepository.AddAsync(persona);
        }

        public async Task UpdatePersonaAsync(DimPersona persona)
        {
            await _personaRepository.UpdateAsync(persona);
        }

        public async Task DeletePersonaAsync(DimPersona persona)
        {
            await _personaRepository.DeleteAsync(persona);
        }
    }
}
