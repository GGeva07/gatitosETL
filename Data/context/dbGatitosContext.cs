using Microsoft.EntityFrameworkCore;
using gatitosEtl.Models.DIMS;

namespace gatitosEtl.Data.context
{
    public class DbGatitosContext : DbContext
    {
        public DbGatitosContext(DbContextOptions<DbGatitosContext> options) : base(options)
        {
        }

        public DbSet<DimCiudad> DimCiudades { get; set; }
        public DbSet<DimPersona> DimPersonas { get; set; }
        public DbSet<DimGato> DimGatos { get; set; }
        public DbSet<DimFecha> DimFechas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DimCiudad>()
                .HasKey(c => c.id_ciudad);

            modelBuilder.Entity<DimPersona>()
                .HasKey(p => p.id_persona);

            modelBuilder.Entity<DimGato>()
                .HasKey(g => g.id_gato);

            modelBuilder.Entity<DimFecha>()
                .HasKey(f => f.id_fecha);
        }
    }
}
