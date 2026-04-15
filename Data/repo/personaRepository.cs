using gatitosEtl.Data.context;
using gatitosEtl.Data.interfaces;
using gatitosEtl.Models.DIMS;

namespace gatitosEtl.Data.repo
{

    public class PersonaRepository : GenericRepository<DimPersona>, IPersonaRepository
    {
        public PersonaRepository(DbGatitosContext context) : base(context)
        {
        }
    }
}