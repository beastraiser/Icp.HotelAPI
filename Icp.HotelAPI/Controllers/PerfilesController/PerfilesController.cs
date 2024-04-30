using AutoMapper;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context.Entidades;
using Icp.HotelAPI.Controllers.PerfilesController.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Icp.HotelAPI.Controllers.PerfilesController
{
    [ApiController]
    [Route("api/perfiles")]
    public class PerfilesController : ControllerBase
    {
        private readonly FCT_ABR_11Context context;
        private readonly IMapper mapper;

        public PerfilesController(FCT_ABR_11Context context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        // Obtener los perfiles
        [HttpGet]
        public async Task<ActionResult<List<PerfilDTO>>> Get()
        {
            var entidades = await context.Perfils.ToListAsync();
            var dtos = mapper.Map<List<PerfilDTO>>(entidades);
            return dtos;
        }

        // Obtener perfil por {id}
        [HttpGet("{id}", Name = "obtenerPerfil")]
        public async Task<ActionResult<PerfilDTO>> Get(int id)
        {
            var entidad = await context.Perfils.FirstOrDefaultAsync(x => x.Id == id);

            if (entidad == null)
            {
                return NotFound();
            }

            var dto = mapper.Map<PerfilDTO>(entidad);
            return dto;
        }

        // Introducir un nuevo perfil
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] PerfilCreacionDTO perfilCreacionDTO)
        {
            var existeTipo = await context.Perfils.AnyAsync(x => x.Tipo == perfilCreacionDTO.Tipo);

            if (existeTipo)
            {
                return BadRequest($"Ya existe un perfil tipo {perfilCreacionDTO.Tipo}");
            }

            var entidad = mapper.Map<Perfil>(perfilCreacionDTO);
            context.Add(entidad);
            await context.SaveChangesAsync();
            var entidadDTO = mapper.Map<PerfilCreacionDTO>(entidad);
            return new CreatedAtRouteResult("obtenerPerfil", new { id = entidad.Id }, entidadDTO);
        }

        // Cambiar datos perfil por {id}
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] PerfilCreacionDTO perfilCreacionDTO)
        {
            var entidad = mapper.Map<Perfil>(perfilCreacionDTO);
            entidad.Id = id;
            context.Entry(entidad).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }

        // Borrar perfil por {id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Perfils.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new Perfil() { Id = id });
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
