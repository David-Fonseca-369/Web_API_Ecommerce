using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_Ecommerce.DTOs.Producto
{
    public class ProductoDTO
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public int IdCategoria { get; set; }
        public string NombreCategoria { get; set; }
        public string Codigo { get; set; }
        public decimal PrecioVenta { get; set; }
        public int Stock { get; set; }
        public int Vendidos { get; set; }
        public bool Estado { get; set; }
    }
}
