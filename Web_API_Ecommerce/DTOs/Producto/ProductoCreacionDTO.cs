using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_Ecommerce.DTOs.Producto
{
    public class ProductoCreacionDTO
    {
        [Required]
        public int IdCategoria { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Codigo { get; set; }
        [Required]
        public decimal PrecioVenta { get; set; }
        [Required]
        public int Stock { get; set; }
        public string Descripcion { get; set; }
        [Required]
        public IFormFile Portada { get; set; }

        //Imagenes
        public IList<IFormFile> Imagenes { get; set; }
    }
}
