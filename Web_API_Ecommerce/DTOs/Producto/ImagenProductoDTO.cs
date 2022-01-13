using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_Ecommerce.DTOs.Producto
{
    public class ImagenProductoDTO
    {
        public int IdImagen { get; set; }
        public string Nombre { get; set; }
        public string Ruta { get; set; }
    }
}
