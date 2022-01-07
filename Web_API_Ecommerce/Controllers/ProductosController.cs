using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_API_Ecommerce.DTOs;
using Web_API_Ecommerce.DTOs.Producto;
using Web_API_Ecommerce.Entities;
using Web_API_Ecommerce.Helpers;

namespace Web_API_Ecommerce.Controllers
{
    [Route("api/productos")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly IMapper mapper;
        private readonly string contenedor = "imagenesProducto";
        


        public ProductosController(ApplicationDbContext context, IAlmacenadorArchivos almacenadorArchivos, IMapper mapper)
        {
            this.context = context;
            this.almacenadorArchivos = almacenadorArchivos;
            this.mapper = mapper;
        }

        //POST: api/productos/crear
        [HttpPost("crear")]
        public async Task<ActionResult> Crear([FromForm] ProductoCreacionDTO productoCreacionDTO)
        {

            string rutaImagenPortada = await almacenadorArchivos.GuardarArchivo(contenedor, productoCreacionDTO.Portada);
            Producto producto = new()
            {
                IdCategoria = productoCreacionDTO.IdCategoria,
                Nombre = productoCreacionDTO.Nombre,
                Codigo = productoCreacionDTO.Codigo,
                PrecioVenta = productoCreacionDTO.PrecioVenta,
                Stock = productoCreacionDTO.Stock,
                Vendidos = 0,
                Descripcion = productoCreacionDTO.Descripcion,
                Portada = rutaImagenPortada,
                Estado = true
            };

            context.Add(producto);

            await context.SaveChangesAsync();

            //Verifico si hay imagenes
            if (productoCreacionDTO.Imagenes == null)
            {
                return NoContent();
            }

            var idProducto = producto.IdProducto;

            foreach (var imagen in productoCreacionDTO.Imagenes)
            {
                string rutaImagen = await almacenadorArchivos.GuardarArchivo(contenedor, imagen);

                ImagenProducto imagenProducto = new ImagenProducto
                {
                    IdProducto = idProducto,
                    Nombre = imagen.FileName,
                    Ruta = rutaImagen
                };

                context.Add(imagenProducto);
            }

            await context.SaveChangesAsync();

            return NoContent();

        }

        //GET: api/productos/todosPaginacion
        [HttpGet("todosPaginacion")]
        public async Task<ActionResult<List<ProductoDTO>>> TodosPaginacion([FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryable = context.Productos.Include(x => x.Categoria).AsQueryable();

            await HttpContext.InsertarParametrosPaginacionEnCabecera(queryable);

            var productosPaginacion = await queryable.Paginar(paginacionDTO).ToListAsync();

            return productosPaginacion.Select(x => new ProductoDTO
            {
                IdProducto = x.IdProducto,
                Nombre = x.Nombre,
                IdCategoria = x.IdCategoria,
                NombreCategoria = x.Categoria.Nombre,
                Codigo = x.Codigo,
                PrecioVenta = x.PrecioVenta,
                Stock = x.Stock,
                Vendidos = x.Vendidos,
                Portada = x.Portada,
                Estado = x.Estado
            }).ToList();

        }

        //GET: api/productos/cardsPaginacion
        [HttpGet("cardsPaginacion")]
        public async Task<ActionResult<List<ProductoCardDTO>>> CardsPaginacion([FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryable = context.Productos.Where(x => x.Estado).AsQueryable();

            await HttpContext.InsertarParametrosPaginacionEnCabecera(queryable);

            var productosPaginacion = await queryable.Paginar(paginacionDTO).ToListAsync();

            return productosPaginacion.Select( x => new ProductoCardDTO
            {
                IdProducto = x.IdProducto,
                Nombre = x.Nombre,
                PrecioVenta = x.PrecioVenta,
                Vendidos = x.Vendidos,
                Portada = x.Portada

            }).ToList();

        }

        //PUT: api/productos/desactivar/{id} 
        [HttpPut("desactivar/{id:int}")]
        public async Task<ActionResult>Desactivar([FromRoute] int id)
        {
            var producto = await context.Productos.FirstOrDefaultAsync(x => x.IdProducto == id);

            if (producto == null)
            {
                return NotFound();
            }

            producto.Estado = false;

            await context.SaveChangesAsync();

            return NoContent();
        }

        //PUT: api/productos/activar/{id} 
        [HttpPut("activar/{id:int}")]
        public async Task<ActionResult>Activar([FromRoute] int id)
        {
            var producto = await context.Productos.FirstOrDefaultAsync(x => x.IdProducto == id);

            if (producto == null)
            {
                return NotFound();
            }

            producto.Estado = true;

            await context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<string> ObtenerImagen(int idProducto)
        {
            var imagen = await context.ImagenesProducto.FirstOrDefaultAsync(x => x.IdProducto == idProducto);

            if (imagen == null)
            {
                return null;
            }

            return imagen.Ruta;
        }


    }
}
