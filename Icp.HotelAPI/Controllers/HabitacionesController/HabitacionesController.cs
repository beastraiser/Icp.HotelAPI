using AutoMapper;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context.Entidades;
using Icp.HotelAPI.Controllers.HabitacionesController.DTO;
using Icp.HotelAPI.Servicios.HabitacionesService.Interfaces;
using Icp.HotelAPI.ServiciosCompartidos.PaginacionDTO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Icp.HotelAPI.Controllers.HabitacionesController
{
    [ApiController]
    [Route("api/habitaciones")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class HabitacionesController : CustomBaseController.CustomBaseController
    {
        private readonly FCT_ABR_11Context context;
        private readonly IMapper mapper;
        private readonly IHabitacionService habitacionService;

        public HabitacionesController(
            FCT_ABR_11Context context, 
            IMapper mapper,
            IHabitacionService habitacionService) 
            : base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
            this.habitacionService = habitacionService;
        }

        
        // Obtener todas las habitaciones
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<HabitacionDTO>>> ObtenerHabitaciones()
        {
            return await Get<Habitacion, HabitacionDTO>();
        }

        // Obtener habitacion por {id}
        [HttpGet("{id}", Name = "obtenerHabitacion")]
        [AllowAnonymous]
        public async Task<ActionResult<HabitacionDTO>> ObtenerHabitacionesPorId(int id)
        {
            return await Get<Habitacion, HabitacionDTO>(id);
        }

        // Obtener habitacion por fecha-inicio, fecha-fin y maximo personas
        [HttpPost("fechas")]
        [AllowAnonymous]
        public async Task<ActionResult<List<HabitacionDTO>>> ObtenerHabitacionesDisponibles([FromBody] DisponibilidadRequestDTO disponibilidadRequestDTO)
        {
            var habitacionesDisponibles = await habitacionService.ObtenerHabitacionesDisponiblesAsync(disponibilidadRequestDTO);
            return Ok(habitacionesDisponibles);
        }

        // Obtener todas las habitaciones disponibles con paginación (10 resultados máximo por página)
        [HttpGet("disponibles")]
        [AllowAnonymous]
        public async Task<ActionResult<List<HabitacionDTO>>> Get([FromQuery] PaginacionDTO paginacionDTO)
        {
            return await Get<Habitacion, HabitacionDTO>(paginacionDTO);
        }

        // Filtro por categoria y disponibilidad = true con paginación
        [HttpGet("categoria")]
        [AllowAnonymous]
        public async Task<ActionResult<List<HabitacionDTO>>> Filtrar([FromQuery] FiltroHabitacionDTO filtroHabitacionDTO)
        {
            return await habitacionService.Filtrar(filtroHabitacionDTO);
        }

        // Introducir una nueva habitacion
        [HttpPost("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult> CrearNuevaHabitacion([FromBody] HabitacionDTO habitacionDTO, int id)
        {
            return await Post<HabitacionDTO, Habitacion, HabitacionDTO>(habitacionDTO, "obtenerHabitacion", id);
        }

        // Cambiar datos habitacion por id
        [HttpPut("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult> CambiarDatosHabitacion(int id, [FromBody] HabitacionPatchDTO habitacionDTO)
        {
            return await Put<HabitacionPatchDTO, Habitacion>(habitacionDTO, id);
        }

        // Cambiar disponibilidad/categoria
        [HttpPatch("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult> CambiarCampoHabitacion(int id, JsonPatchDocument<HabitacionPatchDTO> patchDocument)
        {
            return await Patch<Habitacion, HabitacionPatchDTO>(id, patchDocument);
        }

        // Borrar habitacion por {id}
        [HttpDelete("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult> BorrarHabitacion(int id)
        {
            return await Delete<Habitacion>(id);
        }
    }
}
