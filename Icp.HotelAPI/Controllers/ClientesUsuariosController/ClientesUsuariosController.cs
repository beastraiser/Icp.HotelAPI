using AutoMapper;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context.Entidades;
using Icp.HotelAPI.Controllers.ClientesUsuariosController.DTO;
using Icp.HotelAPI.Controllers.UsuariosController.DTO;
using Icp.HotelAPI.Servicios.ClientesUsuariosService.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Icp.HotelAPI.Controllers.ClientesUsuariosController
{
    [ApiController]
    [Route("api/registro")]
    public class ClientesController : CustomBaseController.CustomBaseController
    {
        private readonly FCT_ABR_11Context context;
        private readonly IMapper mapper;
        private readonly IClienteUsuarioService clientesUsuariosService;

        public ClientesController(FCT_ABR_11Context context, IMapper mapper, IClienteUsuarioService clientesUsuariosService) : base(context, mapper)
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
        public async Task<ActionResult<VClienteUsuarioDetallesUsuarioDTO>> Get(int id)
        {
            var entidad = await context.VClienteUsuarios.FirstOrDefaultAsync(x => x.IdCliente == id);
            if (entidad == null)
            {
                return NotFound();
            }
            return mapper.Map<VClienteUsuarioDetallesUsuarioDTO>(entidad);
        }

        // Obtener datos cliente-usuario por {idUsuario}
        [HttpGet("usuario/{id}", Name = "obtenerClienteUsuario")]
        public async Task<ActionResult<VClienteUsuarioDetallesClienteDTO>> Get2(int id)
        {
            var entidad = await context.VClienteUsuarios.FirstOrDefaultAsync(x => x.IdUsuario == id);
            if (entidad == null)
            {
                return NotFound();
            }
            return mapper.Map<VClienteUsuarioDetallesClienteDTO>(entidad);
        }

        // Registrar nuevo usuario
        [HttpPost]
        public async Task<ActionResult<RespuestaAutenticacionDTO>> Registrar([FromBody] ClienteUsuarioDTO clienteUsuarioDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var exito = await clientesUsuariosService.Registrar(clienteUsuarioDTO);

            if (exito)
            {
                return Ok("Cliente y usuario creados correctamente.");
            }
            else
            {
                return StatusCode(500, "Error al crear cliente y usuario.");
            }
        }
    }
}