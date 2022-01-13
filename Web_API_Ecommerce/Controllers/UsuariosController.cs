using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Web_API_Ecommerce.DTOs;
using Web_API_Ecommerce.DTOs.Usuario;
using Web_API_Ecommerce.Entities;
using Web_API_Ecommerce.Helpers;

namespace Web_API_Ecommerce.Controllers
{
    [Route("api/usuarios")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;

        public UsuariosController(ApplicationDbContext context, IConfiguration configuration, IMapper mapper)
        {
            this.context = context;
            this.configuration = configuration;
            this.mapper = mapper;
        }
        /// <summary>
        /// Los usuarios tienene un rol:
        /// 1 = Admnistrador
        /// 2 = Vendedor
        /// 3 = Cliente
        /// </summary>
        /// <param name="usuarioCreacionDTO"></param>
        /// <returns></returns>
        /// 

        //GET : api/usuarios/usuariosPaginacion
        [HttpGet("usuariosPaginacion")]
        public async Task<ActionResult<List<UsuarioDTO>>> UsuariosPaginacion([FromQuery] PaginacionDTO paginacionDTO)
        {
            var usuarios = await context.Usuarios.Where(x => x.IdRol == 1 ||x.IdRol == 2).ToListAsync();

            int cantidad = usuarios.Count();

            HttpContext.InsertarParametrosPaginacionEnCabeceraPersonalizado(cantidad);

            var queryable = usuarios.AsQueryable();
            var usuariosPaginado = queryable.Paginar(paginacionDTO).ToList();

            return mapper.Map<List<UsuarioDTO>>(usuariosPaginado);
        }
        
        
        //GET : api/usuarios/clientesPaginacion
        [HttpGet("clientesPaginacion")]
        public async Task<ActionResult<List<UsuarioDTO>>> ClientesPaginacion([FromQuery] PaginacionDTO paginacionDTO)
        {
            var usuarios = await context.Usuarios.Where(x => x.IdRol == 3).ToListAsync();

            int cantidad = usuarios.Count();

            HttpContext.InsertarParametrosPaginacionEnCabeceraPersonalizado(cantidad);

            var queryable = usuarios.AsQueryable();
            var usuariosPaginado = queryable.Paginar(paginacionDTO).ToList();

            return mapper.Map<List<UsuarioDTO>>(usuariosPaginado);
        }

        //GET: api/usuarios/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<UsuarioDTO>>Get([FromRoute] int id)
        {
            var usuario = await context.Usuarios.FirstOrDefaultAsync(x => x.IdUsuario == id);

            if (usuario == null)
            {
                return NotFound();
            }

            return mapper.Map<UsuarioDTO>(usuario);

        }




        //POST: api/usuarios/crearUsuario
        [HttpPost("crearUsuario")]
        public async Task<ActionResult> CrearUsuario([FromBody] UsuarioCreacionDTO usuarioCreacionDTO)
        {

            //Verificar si el email existe 
            var correo = usuarioCreacionDTO.Correo.ToLower();

            if (await context.Usuarios.AnyAsync(x => x.Correo == correo))
            {
                return BadRequest("El correo ya existe.");
            }

            EncryptPassword.CrearPasswordHash(usuarioCreacionDTO.Password, out byte[] passwordHash, out byte[] passwordSalt);

            Usuario usuario = new()
            {
                IdRol = usuarioCreacionDTO.IdRol,
                Nombre = usuarioCreacionDTO.Nombre,
                Correo = usuarioCreacionDTO.Correo,
                Password_hash = passwordHash,
                Password_salt = passwordSalt,
                Estado = true
            };

            context.Add(usuario);

            await context.SaveChangesAsync();

            return NoContent();

        }

        //PUT: api/usuarios/editar/{id}
        [HttpPut("editar/{id:int}")]
        public async Task<ActionResult>Editar([FromRoute]int id, [FromBody] UsuarioActualizarDTO usuarioActualizacionDTO)
        {
            var usuario = await context.Usuarios.FirstOrDefaultAsync(x => x.IdUsuario == id);

            if (usuario == null)
            {
                return NotFound();
            }

            var correo = usuarioActualizacionDTO.Correo.ToLower();

            if (correo != usuario.Correo)//Verificar que el correo nuevo no se repita con otro registro
            {
                if (await context.Usuarios.AnyAsync(x => x.Correo == correo))
                {
                    return BadRequest("El correo ya existe.");
                }
            }

            usuario.IdRol = usuarioActualizacionDTO.IdRol;
            usuario.Nombre = usuarioActualizacionDTO.Nombre;
            usuario.Correo = usuarioActualizacionDTO.Correo;


            //Verificar el cambio de contraseña
            if (!string.IsNullOrEmpty(usuarioActualizacionDTO.Password))
            {
                EncryptPassword.CrearPasswordHash(usuarioActualizacionDTO.Password, out byte[] passwordHash, out byte[] passwordSalt);

                usuario.Password_hash = passwordHash;
                usuario.Password_salt = passwordSalt;
            }

            await context.SaveChangesAsync();

            return NoContent();
        }


        //POST: api/usuarios/crearCliente
        [HttpPost("crearCliente")]
        public async Task<ActionResult<RespuestaAutenticacionDTO>> CrearCliente([FromBody] ClienteCreacionDTO clienteCreacionDTO)
        {

            //Verificar si el email existe 
            var correo = clienteCreacionDTO.Correo.ToLower();

            if (await context.Usuarios.AnyAsync(x => x.Correo == correo))
            {
                return BadRequest("El correo ya existe.");
            }

            EncryptPassword.CrearPasswordHash(clienteCreacionDTO.Password, out byte[] passwordHash, out byte[] passwordSalt);

            Usuario usuario = new()
            {
                IdRol = 3,
                Nombre = clienteCreacionDTO.Nombre,
                Correo = clienteCreacionDTO.Correo,
                Password_hash = passwordHash,
                Password_salt = passwordSalt,
                Estado = true
            };

            context.Add(usuario);

            await context.SaveChangesAsync();

            return ConstruirToken(usuario);

        }

        //POST: api/usuarios/login
        [HttpPost("login")]
        public async Task<ActionResult<RespuestaAutenticacionDTO>> Login(LoginDTO loginDTO)
        {
            var correo = loginDTO.Correo.ToLower();

            var usuario = await context.Usuarios.FirstOrDefaultAsync(x => x.Correo == correo && x.Estado);
            if (usuario == null)
            {
                return NotFound("No pudimos encontrar tu cuenta o ha sido deshabilitada.");
            }

            //Verififcar contraseña
            if (!VerificarPasswordHash(loginDTO.Password, usuario.Password_hash, usuario.Password_salt))
            {
                return BadRequest("Login incorrecto.");
            }

            return ConstruirToken(usuario);

        }




        [HttpPut("activar/{id:int}")]
        public async Task<ActionResult> Activar(int id)
        {
            var usuario = await context.Usuarios.FirstOrDefaultAsync(x => x.IdUsuario == id);

            if (usuario == null)
            {
                return NotFound();
            }

            usuario.Estado = true;

            await context.SaveChangesAsync();

            return NoContent();
        }
        
        [HttpPut("desactivar/{id:int}")]
        public async Task<ActionResult> Desactivar(int id)
        {
            var usuario = await context.Usuarios.FirstOrDefaultAsync(x => x.IdUsuario == id);

            if (usuario == null)
            {
                return NotFound();
            }

            usuario.Estado = false;
            await context.SaveChangesAsync();

            return NoContent();
        }

        private RespuestaAutenticacionDTO ConstruirToken(Usuario usuario)
        {
            var claims = new List<Claim>()
            {
                new Claim("idUsuario",usuario.IdUsuario.ToString()),
                new Claim("rol",usuario.IdRol == 1 ? "Administrador" : usuario.IdRol == 2 ? "Vendedor" : usuario.IdRol == 3 ?"Cliente" :"Indefinido"),
                new Claim("correo", usuario.Correo)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["keyjwt"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddHours(1);

            var token = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expires, signingCredentials: credentials);

            return new RespuestaAutenticacionDTO()
            {
                Nombre = usuario.Nombre,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiracion = expires
            };
        }

        private bool VerificarPasswordHash(string password, byte[] passwordHashAlmacenado, byte[] passwordSalt)
        {
            //Recibe el password, lo encripta y lo compara con el password almacenado.

            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var passwordHashNuevo = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return new ReadOnlySpan<byte>(passwordHashAlmacenado).SequenceEqual(new ReadOnlySpan<byte>(passwordHashNuevo)); //Compara
            }
        }
    }
}
