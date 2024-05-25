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
using Icp.HotelAPI.Servicios.UsuariosService.Interfaces;

namespace Icp.HotelAPI.Controllers.UsuariosController
{
    [ApiController]
    [Route("api/usuarios")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsuariosController : CustomBaseController.CustomBaseController
    {
        private readonly FCT_ABR_11Context context;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;
        private readonly ILoginService loginService;
        private readonly IUsuarioService usuarioService;

        public UsuariosController(
            FCT_ABR_11Context context, 
            IMapper mapper, 
            IConfiguration configuration,
            ILoginService loginService,
            IUsuarioService usuarioService) : base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
            this.configuration = configuration;
            this.loginService = loginService;
            this.usuarioService = usuarioService;
        }

        // Obtener todos los usuarios
        [HttpGet]
        [AllowAnonymous]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "RECEPCION")]
        public async Task<ActionResult<List<UsuarioDTO>>> ObtenerUsuarios()
        {
            return await Get<Usuario, UsuarioDTO>();
        }

        // Obtener usuarios por {id}
        [HttpGet("{id}", Name = "obtenerUsuario")]
        [AllowAnonymous]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "RECEPCION")]
        public async Task<ActionResult<UsuarioDTO>> ObtenerUsuariosPorId(int id)
        {
            return await Get<Usuario, UsuarioDTO>(id);
        }

        // Renovar Token
        [HttpGet("RenovarToken")]
        [AllowAnonymous]
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
            try
            {
                var respuesta = await usuarioService.Login(usuarioCredencialesDTO);
                return Ok(respuesta);
            }
            catch (InvalidOperationException ex)
            {
                return Unauthorized(new { ex.Message });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Ocurrió un error inesperado. Por favor, intente de nuevo más tarde." });
            }
        }

        // Introducir un nuevo usuario
        [HttpPost]
        [AllowAnonymous]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "ADMIN")]
        public async Task<ActionResult> CrearNuevoUsuario([FromBody] UsuarioCreacionDTO usuarioCreacionDTO, int id)
        {
            return await Post<UsuarioCreacionDTO, Usuario, UsuarioDTO>(usuarioCreacionDTO, "obtenerUsuario", id);
        }

        // Cambiar datos usuario
        [HttpPut("{id}")]
        [AllowAnonymous]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "CLIENTE")]
        public async Task<ActionResult> CambiarDatosUsuario(int id, [FromBody] UsuarioCreacionDTO usuarioCreacionDTO)
        {
            return await Put<UsuarioCreacionDTO, Usuario>(usuarioCreacionDTO, id);
        }

        // Cambiar un dato especifico
        [HttpPatch("{id}")]
        [AllowAnonymous]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "CLIENTE")]
        public async Task<ActionResult> CambiarCampoUsuario(int id, JsonPatchDocument<UsuarioCreacionDTO> patchDocument)
        {
            return await Patch<Usuario, UsuarioCreacionDTO>(id, patchDocument);
        }

        // Borrar usuario y cliente asociado
        [HttpDelete("{id}")]
        [AllowAnonymous]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "RECEPCION")]
        public async Task<ActionResult> BorrarUsuario(int id)
        {
            var borrado = await usuarioService.BorrarUsuario(id);
            if (borrado)
            {
                return Ok("El usuario ha sido eliminado correctamente.");
            }
            return BadRequest("El usuario no existe");
        }
    }
}
