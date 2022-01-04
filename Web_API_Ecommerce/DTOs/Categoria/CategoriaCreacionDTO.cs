using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_Ecommerce.DTOs.Categoria
{
    public class CategoriaCreacionDTO
    {
        [Required]
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }
}
