using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_Ecommerce.DTOs.Venta
{
    public class DetalleDTO
    {
        public int IdDetalle { get; set; }
        public int IdCabecera { get; set; }
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
    }
}
