using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_API_Ecommerce.Entities;
using Web_API_Ecommerce.Mapping;

namespace Web_API_Ecommerce
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new CategoriaMap());
            modelBuilder.ApplyConfiguration(new ProductoMap());
            modelBuilder.ApplyConfiguration(new ImagenProductoMap());
            modelBuilder.ApplyConfiguration(new UsuarioMap());
            modelBuilder.ApplyConfiguration(new VentaCabeceraMap());
            modelBuilder.ApplyConfiguration(new VentaDetalleMap());
        }

        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<ImagenProducto> ImagenesProducto { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<VentaCabecera> VentasCabecera { get; set; }
        public DbSet<VentaDetalle> VentaDetalles { get; set; }
    }
}
