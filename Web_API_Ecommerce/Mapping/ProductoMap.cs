using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_API_Ecommerce.Entities;

namespace Web_API_Ecommerce.Mapping
{
    public class ProductoMap : IEntityTypeConfiguration<Producto>
    {
        public void Configure(EntityTypeBuilder<Producto> builder)
        {
            builder.ToTable("Producto")
                .HasKey(x => x.IdProducto);

            builder.HasOne(x => x.Categoria).WithMany().HasForeignKey(x => x.IdCategoria);

        }
    }
}
