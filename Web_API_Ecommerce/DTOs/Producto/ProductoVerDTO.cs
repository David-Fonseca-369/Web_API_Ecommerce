using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_API_Ecommerce.Entities;

namespace Web_API_Ecommerce.DTOs.Producto
{
    public class ProductoVerDTO
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public int IdCategoria { get; set; }
        public string Codigo { get; set; }
        public decimal PrecioVenta { get; set; }
        public int Stock { get; set; }
        public int Vendidos { get; set; }
        public string Descripcion { get; set; }
        public string Portada { get; set; }

        //Lista de imagenes
        public List<ImagenProductoDTO>  Imagenes { get; set; }

    }
}
