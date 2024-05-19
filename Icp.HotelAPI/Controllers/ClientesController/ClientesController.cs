using AutoMapper;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context.Entidades;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Icp.HotelAPI.Controllers.ClientesController.DTO;

namespace Icp.HotelAPI.Controllers.ClientesController
{
    [ApiController]
    [Route("api/clientes")]
    public class ClientesController : CustomBaseController.CustomBaseController
    {
        private readonly FCT_ABR_11Context context;
        private readonly IMapper mapper;

        public ClientesController(FCT_ABR_11Context context, IMapper mapper) : base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
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
