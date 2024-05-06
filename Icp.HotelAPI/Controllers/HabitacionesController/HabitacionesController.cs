using AutoMapper;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context.Entidades;
using Icp.HotelAPI.Controllers.CategoriasController.DTO;
using Icp.HotelAPI.Controllers.HabitacionesController.DTO;
using Icp.HotelAPI.ServiciosCompartidos.PaginacionDTO;
using Icp.HotelAPI.ServiciosCompartidos.PaginacionDTO.Helpers;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Icp.HotelAPI.Controllers.HabitacionesController
{
    [ApiController]
    [Route("api/habitaciones")]
    public class HabitacionesController : ControllerBase
    {
        private readonly FCT_ABR_11Context context;
        private readonly IMapper mapper;

        public HabitacionesController(FCT_ABR_11Context context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        // Obtener todas las habitaciones sin paginación
        [HttpGet]
        public async Task<ActionResult<List<HabitacionDTO>>> Get2()
        {
            var entidades = await context.Habitaciones.ToListAsync();
            var dtos = mapper.Map<List<HabitacionDTO>>(entidades);
            return dtos;
        }

        // Obtener todas las habitaciones disponibles con paginación (10 resultados máximo por página)
        [HttpGet("disponibles")]
        public async Task<ActionResult<List<HabitacionDTO>>> Get([FromQuery] PaginacionDTO paginacionDTO)
        {
            var disponible = context.Habitaciones
                .Where(x => x.Disponibilidad == true)
                .AsQueryable();

            var queryable = disponible.AsQueryable();
            await HttpContext.InsertarParametrosPaginacion(queryable, paginacionDTO.CantidadRegistrosPorPagina);

            var entidades = await queryable.Paginar(paginacionDTO).ToListAsync();
            var dtos = mapper.Map<List<HabitacionDTO>>(entidades);
            return dtos;
        }

        // Filtro por categoria y disponibilidad = true
        [HttpGet("categoria")]
        public async Task<ActionResult<List<HabitacionDTO>>> Filtrar([FromQuery] FiltroHabitacionDTO filtroHabitacionDTO)
        {
            var habitacionesQueryable = context.Habitaciones
                .Where(x => x.Disponibilidad == true)
                .AsQueryable();

            if (filtroHabitacionDTO.IdCategoria != 0)
            {
                habitacionesQueryable = habitacionesQueryable.Where(x => x.IdCategoria == filtroHabitacionDTO.IdCategoria);
            }

            await HttpContext.InsertarParametrosPaginacion(habitacionesQueryable, filtroHabitacionDTO.CantidadRegistrosPorPagina);

            var habitaciones = await habitacionesQueryable.Paginar(filtroHabitacionDTO.Paginacion).ToListAsync();

            return mapper.Map<List<HabitacionDTO>>(habitaciones);
        }

        // Obtener habitacion por {numero}
        [HttpGet("{numero}", Name = "obtenerHabitacion")]
        public async Task<ActionResult<HabitacionDTO>> Get(int numero)
        {
            var entidad = await context.Habitaciones.FirstOrDefaultAsync(x => x.Numero == numero);

            if (entidad == null)
            {
                return NotFound();
            }

            var dto = mapper.Map<HabitacionDTO>(entidad);
            return dto;
        }

        // Introducir una nueva habitacion
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] HabitacionDTO habitacionDTO)
        {
            var existeHabitacionConElMismoNumero = await context.Habitaciones.AnyAsync(x => x.Numero == habitacionDTO.Numero);

            if (existeHabitacionConElMismoNumero)
            {
                return BadRequest($"Ya existe una habitación con el número {habitacionDTO.Numero}");
            }

            var entidad = mapper.Map<Habitacion>(habitacionDTO);
            context.Add(entidad);
            await context.SaveChangesAsync();
            var entidadDTO = mapper.Map<HabitacionDTO>(entidad);
            return new CreatedAtRouteResult("obtenerHabitacion", entidadDTO);
        }

        // Cambiar datos habitacion
        [HttpPut("{numero}")]
        public async Task<ActionResult> CambiarCategoria(int numero, [FromBody] HabitacionDTO habitacionDTO)
        {
            var entidad = mapper.Map<Habitacion>(habitacionDTO);
            entidad.Numero = (byte)numero;
            context.Entry(entidad).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }

        // Cambiar disponibilidad/categoria
        [HttpPatch("{numero}")]
        public async Task<ActionResult> PatchDisponibilidad(int numero, JsonPatchDocument<HabitacionPatchDTO> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var habitacionDB = await context.Habitaciones.FirstOrDefaultAsync(x => x.Numero == numero);

            // Verifica si el nº de habitacion existe
            if (habitacionDB == null)
            {
                return NotFound();
            }

            var habitacionDTO = mapper.Map<HabitacionPatchDTO>(habitacionDB);
            patchDocument.ApplyTo(habitacionDTO, ModelState);

            var esValido = TryValidateModel(habitacionDTO);

            if (!esValido)
            {
                return BadRequest(ModelState);
            }

            mapper.Map(habitacionDTO, habitacionDB);

            await context.SaveChangesAsync();
            return NoContent();
        }

        // Borrar habitacion
        [HttpDelete("{numero}")]
        public async Task<ActionResult> Delete(int numero)
        {
            var existe = await context.Habitaciones.AnyAsync(x => x.Numero == numero);

            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new Habitacion() { Numero = (byte)numero });
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
