using AutoMapper;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context.Entidades;
using Icp.HotelAPI.Controllers.ClientesUsuariosController.DTO;
using Icp.HotelAPI.Controllers.UsuariosController.DTO;
using Icp.HotelAPI.Servicios.ClientesUsuariosService.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Icp.HotelAPI.Controllers.ClientesUsuariosController
{
    [ApiController]
    [Route("api/registro")]
    public class ClientesController : CustomBaseController.CustomBaseController
    {
        private readonly FCT_ABR_11Context context;
        private readonly IMapper mapper;
        private readonly IClienteUsuarioService clientesUsuariosService;

        public ClientesController(
            FCT_ABR_11Context context, 
            IMapper mapper, 
            IClienteUsuarioService clientesUsuariosService)
            : base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
            this.clientesUsuariosService = clientesUsuariosService;
        }

        // Obtener datos de clientes con usuarios
        [HttpGet("lista")]
        public async Task<ActionResult<List<VClienteUsuarioDTO>>> Get()
        {
            return await Get<VClienteUsuario, VClienteUsuarioDTO>();
        }

        // Obtener datos cliente-usuario por {idCliente}
        [HttpGet("cliente/{id}", Name = "obtenerUsuarioCliente")]
        public async Task<ActionResult<VClienteUsuarioDetallesUsuarioDTO>> ObtenerClienteUsuarioPorIdCliente(int id)
        {
            return await clientesUsuariosService.ObtenerClienteUsuarioPorIdCliente(id);
        }

        // Obtener datos cliente-usuario por {idUsuario}
        [HttpGet("usuario/{id}", Name = "obtenerClienteUsuario")]
        public async Task<ActionResult<VClienteUsuarioDetallesClienteDTO>> ObtenerClienteUsuarioPorIdUsuario(int id)
        {
            try
            {
                return Ok(await clientesUsuariosService.ObtenerClienteUsuarioPorIdUsuario(id));
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

        // Registrar nuevo usuario
        [HttpPost]
        public async Task<ActionResult> Registrar([FromBody] ClienteUsuarioDTO clienteUsuarioDTO)
        {
            try
            {
                await clientesUsuariosService.Registrar(clienteUsuarioDTO);
                return Ok(new { Message = "Registro satisfactorio" });
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
    }
}