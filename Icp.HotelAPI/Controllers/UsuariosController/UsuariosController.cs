using AutoMapper;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context.Entidades;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Icp.HotelAPI.Controllers.UsuariosController.DTO;
using Microsoft.EntityFrameworkCore;

namespace Icp.HotelAPI.Controllers.UsuariosController
{
    [ApiController]
    [Route("api/usuarios")]
    public class UsuariosController : CustomBaseController.CustomBaseController
    {
        private readonly FCT_ABR_11Context context;
        private readonly IMapper mapper;

        public UsuariosController(FCT_ABR_11Context context, IMapper mapper) : base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        // Obtener todos los usuarios
        [HttpGet]
        public async Task<ActionResult<List<UsuarioDTO>>> Get2()
        {
            return await Get<Usuario, UsuarioDTO>();
        }

        // Obtener usuarios por {id}
        [HttpGet("{id}", Name = "obtenerUsuario")]
        public async Task<ActionResult<UsuarioDTO>> Get(int id)
        {
            return await Get<Usuario, UsuarioDTO>(id);
        }

        // Introducir un nuevo usuario
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] UsuarioCreacionDTO usuarioCreacionDTO, int id)
        {
            return await Post<UsuarioCreacionDTO, Usuario, UsuarioDTO>(usuarioCreacionDTO, "obtenerUsuario", id);
        }

        // Cambiar datos usuario
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] UsuarioCreacionDTO usuarioCreacionDTO)
        {
            return await Put<UsuarioCreacionDTO, Usuario>(usuarioCreacionDTO, id);
        }

        // Cambiar un dato especifico
        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<UsuarioCreacionDTO> patchDocument)
        {
            return await Patch<Usuario, UsuarioCreacionDTO>(id, patchDocument);
        }

        // Borrar usuario y cliente asociado
        [HttpDelete("{id}")]
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
