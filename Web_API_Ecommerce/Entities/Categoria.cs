using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_Ecommerce.Entities
{
    public class Categoria
    {
        public int IdCategoria { get; set; }
        [Required]
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool Estado { get; set; }

    }
}
