using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_Ecommerce.DTOs.Usuario
{
    public class LoginDTO
    {
        [Required]
        [EmailAddress]
        public string Correo { get; set; }
        [Required]
        [MinLength(8)]
        public string Password { get; set; }
    }
}
