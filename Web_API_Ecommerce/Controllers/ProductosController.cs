using AutoMapper;
using Microsoft.AspNetCore.Http;
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
            if (await CodigoExiste(productoCreacionDTO.Codigo))
            {
                return BadRequest("Este código ya ha sido registrado.");
            }

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

        //POST : api/productos/agregarImagen/{id}
        [HttpPost("agregarImagen/{id:int}")]
        public async Task<ActionResult> AgregarImagen([FromRoute] int id, [FromForm] List<IFormFile> imagenes)
        {
            if (imagenes.Count > 0)
            {
                foreach (var imagen in imagenes)
                {
                    string rutaImagen = await almacenadorArchivos.GuardarArchivo(contenedor, imagen);


                    ImagenProducto imagenProducto = new ImagenProducto
                    {
                        IdProducto = id,
                        Nombre = imagen.FileName,
                        Ruta = rutaImagen
                    };

                    context.Add(imagenProducto);
                }

                await context.SaveChangesAsync();
            }

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

            return productosPaginacion.Select(x => new ProductoCardDTO
            {
                IdProducto = x.IdProducto,
                Nombre = x.Nombre,
                PrecioVenta = x.PrecioVenta,
                Vendidos = x.Vendidos,
                Portada = x.Portada

            }).ToList();

        }

        //GET: api/productos/cardsFiltrar
        [HttpGet("cardsFiltrar")]
        public async Task<ActionResult<List<ProductoCardDTO>>>CardsFiltrar([FromQuery] ProductoFiltrarDTO productoFiltrarDTO)
        {
            var queryable = context.Productos.Where(x => x.Estado).AsQueryable();

            if (!string.IsNullOrEmpty(productoFiltrarDTO.Nombre))
            {
                queryable = queryable.Where(x => x.Nombre.Contains(productoFiltrarDTO.Nombre));
            }


            await HttpContext.InsertarParametrosPaginacionEnCabecera(queryable);

            var productosPaginacion = await queryable.Paginar(productoFiltrarDTO.PaginacionDTO).ToListAsync();

            return productosPaginacion.Select(x => new ProductoCardDTO
            {
                IdProducto = x.IdProducto,
                Nombre = x.Nombre,
                PrecioVenta = x.PrecioVenta,
                Vendidos = x.Vendidos,
                Portada = x.Portada

            }).ToList();


        }


        //GET: api/productos/descripcion/{id}
        [HttpGet("descripcion/{id:int}")]
        public async Task<ActionResult<ProductoDescripcionDTO>> Descripcion([FromRoute] int id)
        {
            var producto = await context.Productos.FirstOrDefaultAsync(x => x.IdProducto == id);

            if (producto == null)
            {
                return NotFound();
            }

            //obtengo sus imagenes
            var imagenesProducto = await context.ImagenesProducto.Where(x => x.IdProducto == producto.IdProducto).ToListAsync();

            List<string> imagenes = new();

            if (imagenesProducto != null)
            {
                foreach (var item in imagenesProducto)
                {
                    imagenes.Add(item.Ruta);
                }
            }

            return new ProductoDescripcionDTO()
            {
                IdProducto = producto.IdProducto,
                Nombre = producto.Nombre,
                PrecioVenta = producto.PrecioVenta,
                Stock = producto.Stock,
                Vendidos = producto.Vendidos,
                Descripcion = producto.Descripcion,
                Portada = producto.Portada,
                Imagenes = imagenes
            };

        }

        //GET: api/productos/producto/{id}
        [HttpGet("producto/{id:int}")]
        public async Task<ActionResult<ProductoVerDTO>> Producto([FromRoute] int id)
        {
            var producto = await context.Productos.FirstOrDefaultAsync(x => x.IdProducto == id);

            if (producto == null)
            {
                return NotFound();
            }

            var productoMap = mapper.Map<ProductoVerDTO>(producto);

            //Consulto sus imagenes
            var imagenesProducto = await context.ImagenesProducto.Where(x => x.IdProducto == producto.IdProducto).ToListAsync();

            productoMap.Imagenes = imagenesProducto.Select(x => new ImagenProductoDTO() { IdImagen = x.IdImagen, Nombre = x.Nombre, Ruta = x.Ruta }).ToList();

            return productoMap;
        }

        //PUT: api/productos/editar/{id}
        [HttpPut("editar/{id:int}")]
        public async Task<ActionResult> Editar([FromRoute] int id, [FromForm] ProductoEditarDTO productoEditarDTO)
        {
            var producto = await context.Productos.FirstOrDefaultAsync(x => x.IdProducto == id);

            if (producto == null)
            {
                return NotFound();
            }


            if (producto.Codigo != productoEditarDTO.Codigo)
            {
                if (await CodigoExiste(productoEditarDTO.Codigo))
                {
                    return BadRequest("Este código ya ha sido registrado.");
                }
            }

            producto.IdCategoria = productoEditarDTO.IdCategoria;
            producto.Nombre = productoEditarDTO.Nombre;
            producto.Codigo = productoEditarDTO.Codigo;
            producto.PrecioVenta = productoEditarDTO.PrecioVenta;
            producto.Stock = productoEditarDTO.Stock;
            producto.Descripcion = productoEditarDTO.Descripcion;

            if (productoEditarDTO.Portada != null) //Si cambio la portada
            {
                producto.Portada = await almacenadorArchivos.EditarArchivo(contenedor, productoEditarDTO.Portada, producto.Portada);
            }

            await context.SaveChangesAsync();

            return NoContent();
        }


        //PUT: api/productos/desactivar/{id} 
        [HttpPut("desactivar/{id:int}")]
        public async Task<ActionResult> Desactivar([FromRoute] int id)
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
        public async Task<ActionResult> Activar([FromRoute] int id)
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

        //GET: api/productos/imagenesProducto/{id}
        [HttpGet("imagenesProducto/{id}")]
        public async Task<ActionResult<List<ImagenProductoDTO>>> ImagenesProducto([FromRoute] int id)
        {
            var imagenesProducto = await context.ImagenesProducto.Where(x => x.IdProducto == id).ToListAsync();

            if (imagenesProducto == null)
            {
                return NotFound();
            }

            return mapper.Map<List<ImagenProductoDTO>>(imagenesProducto);

        }


        //Delete: api/productos/eliminarImagen/{id}
        [HttpDelete("eliminarImagen/{id:int}")]
        public async Task<ActionResult> EliminarImagen([FromRoute] int id)
        {
            var imagenProducto = await context.ImagenesProducto.FirstOrDefaultAsync(x => x.IdImagen == id);

            if (imagenProducto == null)
            {
                return NotFound();
            }

            await almacenadorArchivos.BorrarArchivo(imagenProducto.Ruta, contenedor);

            context.Remove(imagenProducto);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> CodigoExiste(string codigo)
        {
            return await context.Productos.AnyAsync(x => x.Codigo == codigo);
        }
    }
}
