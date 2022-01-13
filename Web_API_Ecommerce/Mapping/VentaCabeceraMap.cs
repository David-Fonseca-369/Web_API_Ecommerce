using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_API_Ecommerce.Entities;

namespace Web_API_Ecommerce.Mapping
{
    public class VentaCabeceraMap : IEntityTypeConfiguration<VentaCabecera>
    {
        public void Configure(EntityTypeBuilder<VentaCabecera> builder)
        {
            builder.ToTable("ventaCabecera")
                .HasKey(x => x.IdCabecera);

            builder.HasOne(x => x.Usuario).WithMany().HasForeignKey(x => x.IdUsuario);
        }
    }
}
