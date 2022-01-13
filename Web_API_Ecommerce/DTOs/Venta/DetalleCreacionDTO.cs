using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_Ecommerce.DTOs.Venta
{
    public class DetalleCreacionDTO
    {
     
        [Required]
        public int IdProducto { get; set; }
        [Required]
        public int Cantidad { get; set; }
        [Required]
        public decimal Precio { get; set; }

    }
}
