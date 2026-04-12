using app_productoAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace app_productoAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Producto> Productos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Producto>( entity =>
            {
                entity.ToTable("Producto");
                entity.HasKey(e => e.IdProducto);
                entity.Property(e => e.IdProducto).HasColumnName("idProducto");
                entity.Property(e => e.Descripcion).HasColumnName("descripcion");
                entity.Property(e => e.UMedida).HasColumnName("uMedida");
                entity.Property(e => e.Precio).HasColumnName("precio");
                entity.Property(e => e.Stock).HasColumnName("stock");

            });

        }
    }
}
