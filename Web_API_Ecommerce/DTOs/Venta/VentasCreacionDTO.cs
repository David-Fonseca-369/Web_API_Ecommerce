using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_Ecommerce.DTOs.Venta
{
    public class VentasCreacionDTO
    {
        [Required]
        public int IdUsuario { get; set; }
        [Required]
        public string IdTransaccion { get; set; }
        //[Required]
        //public string EstatusPago { get; set; }
        [Required]
        public decimal Importe { get; set; }
        //[Required]
        //public DateTime Fecha { get; set; }
        [Required]
        public string Titular { get; set; }


        //Detalles
        public List<DetalleCreacionDTO> Detalles { get; set; }

    }
}
