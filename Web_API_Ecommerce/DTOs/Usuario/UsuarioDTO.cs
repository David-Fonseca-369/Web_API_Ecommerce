using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_Ecommerce.DTOs.Usuario
{
    public class UsuarioDTO
    {
        public int IdUsuario { get; set; }
        public int IdRol { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public bool Estado { get; set; }

    }
}
