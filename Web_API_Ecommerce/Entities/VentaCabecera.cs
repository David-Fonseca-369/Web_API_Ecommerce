using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_Ecommerce.Entities
{
    public class VentaCabecera
    {
        public int IdCabecera { get; set; }
        [Required]
        public int IdUsuario { get; set; }
        [Required]
        public string IdTransaccion { get; set; }
        [Required]
        public string EstatusPago { get; set; }
        [Required]
        public decimal Importe { get; set; }
        [Required]
        public DateTime Fecha { get; set; }
        [Required]
        public string Titular { get; set; }
        public bool Estado { get; set; }

        //Relaciones
        public Usuario Usuario { get; set; }
    }
}
