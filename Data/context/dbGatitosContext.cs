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

            modelBuilder.Entity<DimCiudad>()
                .Property(c => c.id_ciudad)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<DimCiudad>()
                .Property(c => c.nombre)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<DimPersona>()
                .HasKey(p => p.id_persona);

            modelBuilder.Entity<DimPersona>()
                .Property(p => p.nombre)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<DimPersona>()
                .Property(p => p.idCiudad)
                .IsRequired();

            modelBuilder.Entity<DimPersona>()
                .HasOne<DimCiudad>()  
                .WithMany()
                .HasForeignKey(p => p.idCiudad)
                .OnDelete(DeleteBehavior.Cascade);

            
            modelBuilder.Entity<DimGato>()
                .HasKey(g => g.id_gato);

            modelBuilder.Entity<DimGato>()
                .Property(g => g.nombre)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<DimGato>()
                .Property(g => g.raza)
                .HasMaxLength(50);

            modelBuilder.Entity<DimGato>()
                .Property(g => g.tipo)
                .HasMaxLength(50);

            modelBuilder.Entity<DimFecha>()
                .HasKey(f => f.id_fecha);

            modelBuilder.Entity<DimFecha>()
                .Property(f => f.id_fecha)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<DimFecha>()
                .Property(f => f.dia)
                .IsRequired();

            modelBuilder.Entity<DimFecha>()
                .Property(f => f.mes)
                .IsRequired();

            modelBuilder.Entity<DimFecha>()
                .Property(f => f.anio)
                .IsRequired();

            modelBuilder.Entity<DimCiudad>()
                .HasIndex(c => c.nombre)
                .IsUnique();

            modelBuilder.Entity<DimFecha>()
                .HasIndex(f => new { f.dia, f.mes, f.anio })
                .IsUnique();
        }
    }
}

