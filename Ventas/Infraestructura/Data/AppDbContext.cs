using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Data
{
    public class BdContext : DbContext
    {
        public BdContext(DbContextOptions<BdContext> options)
            : base(options) { }

        // Tablas principales
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Carrito> Carritos { get; set; }
        public DbSet<CarritoItem> CarritoItems { get; set; }
        public DbSet<Orden> Ordenes { get; set; }
        public DbSet<Pago> Pagos { get; set; }
        public DbSet<Tecnico> Tecnicos { get; set; }
        public DbSet<ServicioTecnico> ServiciosTecnicos { get; set; }
        public DbSet<Inventario> Inventarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de precisión para los decimales
            modelBuilder.Entity<Producto>()
                .Property(p => p.Precio)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Orden>()
                .Property(o => o.Total)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Pago>()
                .Property(p => p.Monto)
                .HasColumnType("decimal(18,2)");
        }
    }
}
