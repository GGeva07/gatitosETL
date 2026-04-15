using Dapper;
using gatitosEtl.Data.context;
using gatitosEtl.Data.interfaces;
using gatitosEtl.Models.DIMS;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace gatitosEtl.Data.repo
{
    public class CiudadRepository : GenericRepository<DimCiudad>, ICiudadRepository
    {
        public CiudadRepository(DbGatitosContext context) : base(context)
        {
        }
    }
}