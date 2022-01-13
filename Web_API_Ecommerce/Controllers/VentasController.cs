using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_API_Ecommerce.DTOs;
using Web_API_Ecommerce.DTOs.Venta;
using Web_API_Ecommerce.Entities;
using Web_API_Ecommerce.Helpers;

namespace Web_API_Ecommerce.Controllers
{
    [Route("api/Ventas")]
    [ApiController]
    public class VentasController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public VentasController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        /// <summary>
        /// Estado de entrega:
        /// 
        /// 0 = Pendiente,
        /// 1 = Entregado,
        /// 
        /// </summary>
        /// <param name="ventasCreacionDTO"></param>
        /// <returns></returns>


        [HttpPost("crear")]
        public async Task<ActionResult<int>> Crear([FromBody] VentasCreacionDTO ventasCreacionDTO)
        {
            VentaCabecera venta = new()
            {
                IdUsuario = ventasCreacionDTO.IdUsuario,
                IdTransaccion = ventasCreacionDTO.IdTransaccion,
                EstatusPago = "Completado",
                Importe = ventasCreacionDTO.Importe,
                Fecha = DateTime.Now,
                Titular = ventasCreacionDTO.Titular,
                Estado = false
            };

            context.Add(venta);

            await context.SaveChangesAsync();

            if (ventasCreacionDTO.Detalles.Count > 0)
            {
                int idVenta = venta.IdCabecera;

                foreach (var detalle in ventasCreacionDTO.Detalles)
                {

                    VentaDetalle ventaDetalle = new()
                    {
                        IdCabecera = idVenta,
                        IdProducto = detalle.IdProducto,
                        Cantidad = detalle.Cantidad,
                        Precio = detalle.Precio
                    };

                    context.Add(ventaDetalle);


                    var producto = await context.Productos.FirstOrDefaultAsync(x => x.IdProducto == detalle.IdProducto);
                    //Descontas stock
                    producto.Stock -= detalle.Cantidad;
                    //Sumar vendidos
                    producto.Vendidos += detalle.Cantidad;
                }

                await context.SaveChangesAsync();

            }

            return venta.IdCabecera;
        }


        //GET: api/ventas/comprobante/{id}
        [HttpGet("comprobante/{id:int}")]
        public async Task<ActionResult<VentasDTO>> Comprobante([FromRoute] int id)
        {
            var ventas = await context.VentasCabecera.Include(x => x.Usuario).FirstOrDefaultAsync(x => x.IdCabecera == id);

            if (ventas == null)
            {
                return NotFound();
            }

            var ventaMap = mapper.Map<VentasDTO>(ventas);


            List<DetalleDTO> detallesList = new();

            var detallesVenta = await context.VentaDetalles.Include(x => x.Producto).Where(x => x.IdCabecera == ventas.IdCabecera).ToListAsync();


            foreach (var item in detallesVenta)
            {
                detallesList.Add(new DetalleDTO
                {
                    IdDetalle = item.IdDetalle,
                    IdCabecera = item.IdCabecera,
                    IdProducto = item.IdProducto,
                    NombreProducto = item.Producto.Nombre,
                    Cantidad = item.Cantidad,
                    Precio = item.Precio
                });
            }

            ventaMap.Detalles = detallesList;
            ventaMap.NombreUsuario = ventas.Usuario.Nombre;
            ventaMap.CorreoUsuario = ventas.Usuario.Correo;

            return ventaMap;

        }


        //GET: api/ventas/compras/{idUsuario}
        [HttpGet("compras/{id}")]
        public async Task<ActionResult<List<VentasDTO>>>Compras([FromRoute]int id, [FromQuery] PaginacionDTO paginacionDTO)
        {
            var ventas = await context.VentasCabecera.Where(x => x.IdUsuario == id).OrderByDescending(x => x.IdCabecera).ToListAsync();

            int cantidad = ventas.Count;

            HttpContext.InsertarParametrosPaginacionEnCabeceraPersonalizado(cantidad);

            var queryable = ventas.AsQueryable();
            var ventasPaginado = queryable.Paginar(paginacionDTO).ToList();


            return mapper.Map<List<VentasDTO>>(ventasPaginado);
        }
        
        
        
        //GET: api/ventas/ventasPaginacion
        [HttpGet("ventasPaginacion")]
        public async Task<ActionResult<List<VentasDTO>>>VentasPaginacion([FromQuery] PaginacionDTO paginacionDTO)
        {
            var ventas = await context.VentasCabecera.Include(x => x.Usuario).OrderByDescending(x => x.IdCabecera).ToListAsync();

            int cantidad = ventas.Count;

            HttpContext.InsertarParametrosPaginacionEnCabeceraPersonalizado(cantidad);

            var queryable = ventas.AsQueryable();
            var ventasPaginado = queryable.Paginar(paginacionDTO).ToList();


            var ventasDTO = ventas.Select(x => new VentasDTO {
            IdCabecera = x.IdCabecera,
            IdUsuario = x.IdUsuario,
            NombreUsuario = x.Usuario.Nombre,
            CorreoUsuario = x.Usuario.Correo,
            IdTransaccion = x.IdTransaccion,
            EstatusPago = x.EstatusPago,
            Importe = x.Importe,
            Fecha = x.Fecha,
            Titular = x.Titular,
            Estado = x.Estado
            }).ToList();


            return ventasDTO;
        }

        //PUT : api/ventas/entregar/{idVenta}
        [HttpPut("entregar/{id:int}")]
        public async Task<ActionResult>Entregar([FromRoute] int id)
        {
            var venta = await context.VentasCabecera.FirstOrDefaultAsync(x => x.IdCabecera == id);

            if (venta == null)
            {
                return NotFound();
            }

            venta.Estado = true;

            await context.SaveChangesAsync();

            return NoContent();
        }

    }
}
