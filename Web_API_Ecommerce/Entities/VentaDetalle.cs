using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_Ecommerce.Entities
{
    public class VentaDetalle
    {
        public int IdDetalle { get; set; }
        [Required]
        public int IdCabecera { get; set; }
        [Required]
        public int IdProducto { get; set; }
        [Required]
        public int Cantidad { get; set; }
        [Required]
        public decimal Precio { get; set; }

        //Relaciones
        public VentaCabecera VentaCabecera { get; set; }
        public Producto Producto { get; set; }
    }
}
