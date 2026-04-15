using gatitosEtl.Data.context;
using gatitosEtl.Data.interfaces;
using gatitosEtl.Models.DIMS;

namespace gatitosEtl.Data.repo
{
    public class FechaRepository (DbGatitosContext context) 
    : GenericRepository<DimFecha> (context), IFechaRepository
    {
    }
}
