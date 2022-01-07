using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_Ecommerce.DTOs.Producto
{
    public class ProductoCardDTO
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public decimal PrecioVenta { get; set; }
        public int Vendidos { get; set; }
        public string Portada { get; set; }
    }
}
