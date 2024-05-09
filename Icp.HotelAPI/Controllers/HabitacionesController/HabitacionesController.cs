using AutoMapper;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context.Entidades;
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
    public class HabitacionesController : CustomBaseController.CustomBaseController
    {
        private readonly FCT_ABR_11Context context;
        private readonly IMapper mapper;

        public HabitacionesController(FCT_ABR_11Context context, IMapper mapper) : base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        // Obtener todas las habitaciones
        [HttpGet]
        public async Task<ActionResult<List<HabitacionDTO>>> Get2()
        {
            return await Get<Habitacion, HabitacionDTO>();
        }

        // Obtener habitacion por {id}
        [HttpGet("{id}", Name = "obtenerHabitacion")]
        public async Task<ActionResult<HabitacionDTO>> Get(int id)
        {
            return await Get<Habitacion, HabitacionDTO>(id);
        }

        // Obtener todas las habitaciones disponibles con paginación (10 resultados máximo por página)
        [HttpGet("disponibles")]
        public async Task<ActionResult<List<HabitacionDTO>>> Get([FromQuery] PaginacionDTO paginacionDTO)
        {
            return await Get<Habitacion, HabitacionDTO>(paginacionDTO);
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

        // Introducir una nueva habitacion
        [HttpPost("{id}")]
        public async Task<ActionResult> Post([FromBody] HabitacionDTO habitacionDTO, int id)
        {
            return await Post<HabitacionDTO, Habitacion, HabitacionDTO>(habitacionDTO, "obtenerHabitacion", id);
        }

        // Cambiar datos habitacion
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] HabitacionPatchDTO habitacionDTO)
        {
            return await Put<HabitacionPatchDTO, Habitacion>(habitacionDTO, id);
        }

        // Cambiar disponibilidad/categoria
        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<HabitacionPatchDTO> patchDocument)
        {
            return await Patch<Habitacion, HabitacionPatchDTO>(id, patchDocument);
        }

        // Borrar habitacion
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            return await Delete<Habitacion>(id);
        }
    }
}
