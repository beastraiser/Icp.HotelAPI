using AutoMapper;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context.Entidades;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Icp.HotelAPI.Controllers.ClientesController.DTO;
using Icp.HotelAPI.Servicios.ClientesService.Interfaces;
using Icp.HotelAPI.Controllers.UsuariosController.DTO;
using Microsoft.AspNetCore.Authorization;

namespace Icp.HotelAPI.Controllers.ClientesController
{
    [ApiController]
    [Route("api/clientes")]
    public class ClientesController : CustomBaseController.CustomBaseController
    {
        private readonly FCT_ABR_11Context context;
        private readonly IMapper mapper;
        private readonly IClienteService clienteService;

        public ClientesController(
            FCT_ABR_11Context context, 
            IMapper mapper,
            IClienteService clienteService) 
            : base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
            this.clienteService = clienteService;
        }

        // Obtener todos los clientes
        [HttpGet]
        public async Task<ActionResult<List<ClienteDTO>>> ObtenerClientes()
        {
            return await Get<Cliente, ClienteDTO>();
        }

        // Obtener clientes por {id}
        [HttpGet("{id}", Name = "obtenerCliente")]
        public async Task<ActionResult<ClienteDTO>> ObtenerClientesPorId(int id)
        {
            return await Get<Cliente, ClienteDTO>(id);
        }

        //Obtener clientes por Dni
        [HttpPost("dni")]
        [AllowAnonymous]
        public async Task<ActionResult<ClienteDTO>> ObtenerClientePorDni(ClienteDniDTO clienteDniDTO)
        {
            try
            {
                return await clienteService.ObtenerClientePorDni(clienteDniDTO);
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

        // Introducir un nuevo cliente
        [HttpPost]
        public async Task<ActionResult> CrearNuevoCliente([FromBody] ClienteCreacionDTO clienteCreacionDTO)
        {
            return await Post<ClienteCreacionDTO, Cliente, ClienteDTO>(clienteCreacionDTO, "obtenerCliente", "Dni", "Telefono");
        }

        // Cambiar datos cliente
        [HttpPut("{id}")]
        public async Task<ActionResult> CambiarDatosCliente(int id, [FromBody] ClienteCreacionDTO clienteCreacionDTO)
        {
            return await Put<ClienteCreacionDTO, Cliente>(clienteCreacionDTO, id);
        }

        // Cambiar un dato especifico
        [HttpPatch("{id}")]
        public async Task<ActionResult> CambiarCampoCliente(int id, JsonPatchDocument<ClienteCreacionDTO> patchDocument)
        {
            return await Patch<Cliente, ClienteCreacionDTO>(id, patchDocument);
        }

        // Borrar cliente por {id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> BorrarCliente(int id)
        {
            return await Delete<Cliente>(id);
        }
    }
}
