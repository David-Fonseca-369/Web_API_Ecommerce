using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_API_Ecommerce.DTOs;
using Web_API_Ecommerce.DTOs.Categoria;
using Web_API_Ecommerce.Entities;
using Web_API_Ecommerce.Helpers;

namespace Web_API_Ecommerce.Controllers
{
    [Route("api/categorias")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public CategoriasController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        //GET: api/categorias/todas
        [HttpGet("todas")]
        public async Task<ActionResult<List<CategoriaDTO>>> Todas()
        {
            var categorias = await context.Categorias.Where(x => x.Estado).ToListAsync();
            return mapper.Map<List<CategoriaDTO>>(categorias);
        }

        //GET: api/categorias/TodasPaginacion
        [HttpGet("todasPaginacion")]
        public async Task<ActionResult<List<CategoriaDTO>>> TodasPaginacion([FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryable = context.Categorias.AsQueryable();

            await HttpContext.InsertarParametrosPaginacionEnCabecera(queryable);

            var gruposPaginacion = await queryable.Paginar(paginacionDTO).ToListAsync();

            return mapper.Map<List<CategoriaDTO>>(gruposPaginacion);
        }

        //GET: api/categorias/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CategoriaDTO>> Get(int id)
        {
            var categoria = await context.Categorias.FirstOrDefaultAsync(x => x.IdCategoria == id);

            if (categoria == null)
            {
                return NotFound();
            }

            return mapper.Map<CategoriaDTO>(categoria);
        }


        //POST : api/categorias/crear
        [HttpPost("crear")]
        public async Task<ActionResult> Crear([FromBody] CategoriaCreacionDTO categoriaCreacionDTO)
        {
            var categoria = mapper.Map<Categoria>(categoriaCreacionDTO);
            categoria.Estado = true;

            context.Add(categoria);

            await context.SaveChangesAsync();

            return NoContent();
        }

        //PUT: api/categorias/editar/{id}
        [HttpPut("editar/{id:int}")]
        public async Task<ActionResult> Editar([FromRoute] int id, [FromBody] CategoriaCreacionDTO categoriaCreacionDTO)
        {
            var categoria = await context.Categorias.FirstOrDefaultAsync(x => x.IdCategoria == id);

            if (categoria == null)
            {
                return NotFound();
            }

            //Mapeo grupoCreacionDTO y despues almacenamos ese mapeo en el mismo grupo.
            categoria = mapper.Map(categoriaCreacionDTO, categoria);
            await context.SaveChangesAsync();

            return NoContent();

        }

        //PUT: api/categorias/desactivar/{id}
        [HttpPut("desactivar/{id:int}")]
        public async Task<ActionResult> Desactivar(int id)
        {
            var categoria = await context.Categorias.FirstOrDefaultAsync(x => x.IdCategoria == id);

            if (categoria == null)
            {
                return NotFound();
            }

            categoria.Estado = false;

            await context.SaveChangesAsync();

            return NoContent();

        }


        //PUT: api/categorias/activar/{id}
        [HttpPut("activar/{id:int}")]
        public async Task<ActionResult> Activar(int id)
        {
            var categoria = await context.Categorias.FirstOrDefaultAsync(x => x.IdCategoria == id);

            if (categoria == null)
            {
                return NotFound();
            }

            categoria.Estado = true;

            await context.SaveChangesAsync();

            return NoContent();

        }








    }
}
