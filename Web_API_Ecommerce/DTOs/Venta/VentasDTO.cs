
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API_Ecommerce.DTOs.Venta
{
    public class VentasDTO
    {
        public int IdCabecera { get; set; }
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string CorreoUsuario { get; set; }
        public string IdTransaccion { get; set; }
        public string EstatusPago { get; set; }
        public decimal Importe { get; set; }
        public DateTime Fecha { get; set; }
        public string Titular { get; set; }
        public bool Estado { get; set; }

        public List<DetalleDTO> Detalles { get; set; }
    }
}
