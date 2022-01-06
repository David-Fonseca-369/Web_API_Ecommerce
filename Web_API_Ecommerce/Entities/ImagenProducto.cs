using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_Ecommerce.Entities
{
    public class ImagenProducto
    {
        public int IdImagen { get; set; }
        [Required]
        public int IdProducto { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Ruta { get; set; }

        //Relaciones
        public Producto Producto { get; set; }
    }
}
