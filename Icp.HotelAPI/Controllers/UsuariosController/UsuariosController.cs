using AutoMapper;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context.Entidades;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Icp.HotelAPI.Controllers.UsuariosController.DTO;
using Microsoft.EntityFrameworkCore;
using Icp.HotelAPI.ServiciosCompartidos.LoginService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;

namespace Icp.HotelAPI.Controllers.UsuariosController
{
    [ApiController]
    [Route("api/usuarios")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsuariosController : CustomBaseController.CustomBaseController
    {
        private readonly FCT_ABR_11Context context;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;
        private readonly ILoginService loginService;

        public UsuariosController(FCT_ABR_11Context context, IMapper mapper, 
            IConfiguration configuration,
            ILoginService loginService) : base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
            this.configuration = configuration;
            this.loginService = loginService;
        }

        // Obtener todos los usuarios
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "RECEPCION")]
        public async Task<ActionResult<List<UsuarioDTO>>> Get2()
        {
            return await Get<Usuario, UsuarioDTO>();
        }

        // Obtener usuarios por {id}
        [HttpGet("{id}", Name = "obtenerUsuario")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "RECEPCION")]
        public async Task<ActionResult<UsuarioDTO>> Get(int id)
        {
            return await Get<Usuario, UsuarioDTO>(id);
        }

        // Renovar Token
        [HttpGet("RenovarToken")]
        public ActionResult<RespuestaAutenticacionDTO> Renovar()
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == "email").Value;
            var rol = User.Claims.FirstOrDefault(c => c.Type == "rol").Value;
            var id = User.Claims.FirstOrDefault(c => c.Type == "id").Value;
            var claims = new List<Claim>()
            {
                new Claim("email", email),
                new Claim("rol", rol),
                new Claim("id", id)
            };
            
            return loginService.ConstruirToken(claims);
        }

        // Login
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<RespuestaAutenticacionDTO>> Login(UsuarioCredencialesDTO usuarioCredencialesDTO)
        {
            var resultado = await context.Usuarios.FirstOrDefaultAsync(x => x.Email == usuarioCredencialesDTO.Email);
            resultado.Contrasenya = loginService.HashContrasenya(usuarioCredencialesDTO.Contrasenya);
            await context.SaveChangesAsync();


            if (resultado == null || !loginService.VerificarContrasenya(usuarioCredencialesDTO.Contrasenya, resultado.Contrasenya))
            {
                throw new InvalidOperationException("Usuario no encontrado o contraseña incorrecta");
            }

            var claims = new List<Claim>()
            {
                new Claim("email", usuarioCredencialesDTO.Email),
                new Claim("id", resultado.Id.ToString())
            };

            switch (resultado.IdPerfil)
            {
                case 1:
                    var adminClaim = new Claim("rol", "ADMIN");
                    claims.Add(adminClaim);
                    break;
                case 2:
                    var recepcionClaim = new Claim("rol", "RECEPCION");
                    claims.Add(recepcionClaim);
                    break;
                case 4:
                    var clienteClaim = new Claim("rol", "CLIENTE");
                    claims.Add(clienteClaim);
                    break;
            }

            var token = loginService.ConstruirToken(claims);
            return Ok(token);
        }

        // Introducir un nuevo usuario
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "ADMIN")]
        public async Task<ActionResult> Post([FromBody] UsuarioCreacionDTO usuarioCreacionDTO, int id)
        {
            return await Post<UsuarioCreacionDTO, Usuario, UsuarioDTO>(usuarioCreacionDTO, "obtenerUsuario", id);
        }

        // Cambiar datos usuario
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "CLIENTE")]
        public async Task<ActionResult> Put(int id, [FromBody] UsuarioCreacionDTO usuarioCreacionDTO)
        {
            return await Put<UsuarioCreacionDTO, Usuario>(usuarioCreacionDTO, id);
        }

        // Cambiar un dato especifico
        [HttpPatch("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "CLIENTE")]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<UsuarioCreacionDTO> patchDocument)
        {
            return await Patch<Usuario, UsuarioCreacionDTO>(id, patchDocument);
        }

        // Borrar usuario y cliente asociado
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "RECEPCION")]
        public async Task<ActionResult> Delete(int id)
        {
            var entidad = await context.Usuarios.FirstOrDefaultAsync(x => x.Id == id);

            if (entidad == null)
            {
                return NotFound();
            }

            var clienteUsuario = await context.ClienteUsuarios
                .Where(tc => tc.IdUsuario == id)
                .ToListAsync();

            context.ClienteUsuarios.RemoveRange(clienteUsuario);
            context.Usuarios.Remove(entidad);

            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
