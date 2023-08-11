using Microsoft.EntityFrameworkCore;
using tareas_mvc.Entidades;

namespace tareas_mvc
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<Tarea>().Property(t => t.Titulo).HasMaxLength(250).IsRequired();
        }

        public DbSet<Tarea> Tareas{ get; set; }
        public DbSet<Paso> Pasos { get; set; }
        public DbSet<ArchivoAdjunto> ArchivosAdjuntos{ get; set; }
    }
}
