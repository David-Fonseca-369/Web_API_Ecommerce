using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_Ecommerce.DTOs.Usuario
{
    public class RespuestaAutenticacionDTO
    {
        public string Nombre { get; set; }
        public string Token { get; set; }
        public DateTime Expiracion { get; set; }

    }
}
