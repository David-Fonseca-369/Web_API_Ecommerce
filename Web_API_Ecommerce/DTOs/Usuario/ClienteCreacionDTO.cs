using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_Ecommerce.DTOs.Usuario
{
    public class ClienteCreacionDTO
    {
        [Required]
        [MaxLength(50)]
        public string Nombre { get; set; }
        [Required]
        [EmailAddress]
        [MaxLength(50)]
        public string Correo { get; set; }
        [Required]
        [MinLength(8, ErrorMessage = "La contraseña debe contener mínimo 8 caracteres.")]
        public string Password { get; set; }
    }
}
