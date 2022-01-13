using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_API_Ecommerce.Entities;

namespace Web_API_Ecommerce.Mapping
{
    public class VentaDetalleMap : IEntityTypeConfiguration<VentaDetalle>
    {
        public void Configure(EntityTypeBuilder<VentaDetalle> builder)
        {
            builder.ToTable("ventaDetalle")
                .HasKey(x => x.IdDetalle);

            builder.HasOne(x => x.VentaCabecera).WithMany().HasForeignKey(x => x.IdCabecera);
            builder.HasOne(x => x.Producto).WithMany().HasForeignKey(x => x.IdProducto);

        }
    }
}
