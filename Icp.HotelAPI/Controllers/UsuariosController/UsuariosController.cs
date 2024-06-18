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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "ADMIN")]
        public async Task<ActionResult<List<UsuarioDTO>>> ObtenerUsuarios()
        {
            return await Get<Usuario, UsuarioDTO>();
        }

        // Obtener usuarios por {id}
        [HttpGet("{id}", Name = "obtenerUsuario")]
        public async Task<ActionResult<UsuarioDTO>> ObtenerUsuariosPorId(int id)
        {
            return await Get<Usuario, UsuarioDTO>(id);
        }

        // Dar de baja usuario
        [HttpGet("{id}/baja")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "NOTRECEPCION")]
        public async Task<ActionResult> BajaUsuario(int id)
        {
            try
            {
                var respuesta = await usuarioService.BajaUsuario(id);
                if (respuesta)
                {
                    return Ok();
                }
                return BadRequest(new { Message = "Usuario no encontrado" });
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

        // Dar de alta usuario
        [HttpPut("{id}/altaC")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "ADMIN")]
        public async Task<ActionResult> AltaUsuario(int id, [FromBody] UsuarioAltaDTO usuarioAltaDTO)
        {
            try
            {
                var respuesta = await usuarioService.AltaUsuario(id, usuarioAltaDTO);
                if (respuesta)
                {
                    return Ok();
                }
                return BadRequest();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { ex.Message });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Ocurrió un error inesperado. Por favor, intente de nuevo más tarde." });
            }
        }

        // Dar de alta trabajador
        [HttpGet("{id}/altaT")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "ADMIN")]
        public async Task<ActionResult> AltaTrabajador(int id)
        {
            try
            {
                var respuesta = await usuarioService.AltaTrabajador(id);
                if (respuesta)
                {
                    return Ok();
                }
                return BadRequest();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { ex.Message });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Ocurrió un error inesperado. Por favor, intente de nuevo más tarde." });
            }
        }

        // Obtener usuario por email
        [HttpPost("email")]
        public async Task<ActionResult<UsuarioDTO>> ObtenerUsuarioPorEmail(UsuarioEmailDTO usuarioEmailDTO)
        {
            try
            {
                return await usuarioService.ObtenerUsuarioPorEmail(usuarioEmailDTO);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { ex.Message });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Ocurrió un error inesperado. Por favor, intente de nuevo más tarde." });
            }
        }

        // Verificar datos usuario
        [HttpPost("check")]
        public async Task<ActionResult<UsuarioDTO>> VerificarDatosUsuario(UsuarioCredencialesDTO usuarioCredencialesDTO)
        {
            try
            {
                return await usuarioService.VerificarDatosUsuario(usuarioCredencialesDTO);
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

        // Renovar Token
        [HttpGet("RenovarToken")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "LOGGED")]
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "ADMIN")]
        public async Task<ActionResult<UsuarioDTO>> CrearUsuario([FromBody] UsuarioCreacionDTO usuarioCreacionDTO)
        {
            try
            {
                return Ok(await usuarioService.CrearUsuario(usuarioCreacionDTO));
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

        // Cambiar datos usuario
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "NOTRECEPCION")]
        public async Task<ActionResult> ActualizarUsuario(int id, [FromBody] UsuarioCreacionDTO usuarioCreacionDTO)
        {
            try
            {
                var respuesta = await usuarioService.ActualizarUsuario(id, usuarioCreacionDTO);
                if (respuesta)
                {
                    return Ok();
                }
                return BadRequest();
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

        // Cambiar un dato especifico
        [HttpPatch("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "ADMIN")]
        public async Task<ActionResult> CambiarCampoUsuario(int id, JsonPatchDocument<UsuarioCreacionDTO> patchDocument)
        {
            return await Patch<Usuario, UsuarioCreacionDTO>(id, patchDocument);
        }

        // Borrar usuario y cliente asociado
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "NOTRECEPCION")]
        public async Task<ActionResult> BorrarUsuario(int id)
        {
            var borrado = await usuarioService.BorrarUsuario(id);
            if (borrado)
            {
                return Ok(new { Message = "El usuario ha sido eliminado correctamente" });
            }
            return BadRequest("El usuario no existe");
        }
    }
}
