using CitasMedicas.RecepcionistaApi.Model;
using Microsoft.EntityFrameworkCore;

namespace CitasMedicas.RecepcionistaApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Recepcionista> Recepcionistas { get; set; }
        public DbSet<Correlativo> Correlativos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Recepcionista>().ToTable("Recepcionista");
            modelBuilder.Entity<Correlativo>().ToTable("Correlativo");
            base.OnModelCreating(modelBuilder);
        }
    }
}
