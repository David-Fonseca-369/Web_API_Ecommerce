using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_Ecommerce.DTOs.Usuario
{
    public class UsuarioActualizarDTO
    {
        [Required]
        public int IdRol { get; set; }
        [Required]
        [MaxLength(50)]
        public string Nombre { get; set; }
        [Required]
        [EmailAddress]
        [MaxLength(50)]
        public string Correo { get; set; }
        public string Password { get; set; }
    }
}
