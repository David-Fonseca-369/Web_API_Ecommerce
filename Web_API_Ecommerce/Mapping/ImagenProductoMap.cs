using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_API_Ecommerce.Entities;

namespace Web_API_Ecommerce.Mapping
{
    public class ImagenProductoMap : IEntityTypeConfiguration<ImagenProducto>
    {
        public void Configure(EntityTypeBuilder<ImagenProducto> builder)
        {
            builder.ToTable("imagenProducto")
                .HasKey(x => x.IdImagen);

            builder.HasOne(x => x.Producto).WithMany().HasForeignKey(x => x.IdProducto);
        }
    }
}
