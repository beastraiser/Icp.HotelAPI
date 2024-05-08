using AutoMapper;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context.Entidades;
using Icp.HotelAPI.Controllers.CategoriasController.DTO;
using Icp.HotelAPI.Controllers.ReservasController.DTO;
using Icp.HotelAPI.ServiciosCompartidos.AlmacenadorArchivosLocal.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Icp.HotelAPI.Controllers.ReservasController
{
    [ApiController]
    [Route("api/reservas")]
    public class ReservasController : ControllerBase
    {
        private readonly FCT_ABR_11Context context;
        private readonly IMapper mapper;

        public ReservasController(FCT_ABR_11Context context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        //Obtener todas las reservas con sus habitaciones y servicios
        [HttpGet]
        public async Task<ActionResult<List<ReservaDetallesDTO>>> Get()
        {
            var entidades = await context.Reservas
                .Include(x => x.ReservaHabitacionServicios)
                .ToListAsync();
            var dtos = mapper.Map<List<ReservaDetallesDTO>>(entidades);
            return dtos;
        }

        // Obtener reserva con habitaciones y servicios por id
        [HttpGet("{id}", Name = "obtenerReserva")]
        public async Task<ActionResult<ReservaDetallesDTO>> Get(int id)
        {
            var entidad = await context.Reservas
                .Include(x => x.ReservaHabitacionServicios)
                .FirstOrDefaultAsync(x => x.Id == id);
            
            if (entidad == null)
            {
                return NotFound();
            }

            var dto = mapper.Map<ReservaDetallesDTO>(entidad);
            return dto;
        }

        // Agregar reserva con habitaciones y servicios
        [HttpPost]
        public async Task<ActionResult> Post([FromForm] ReservaDetallesDTO reservaDetallesDTO)
        {
            var habitacionesSolicitadas = reservaDetallesDTO.ReservaHabitacionServicios.Select(hs => hs.IdHabitacion).ToList();

            var existe = await context.ReservaHabitacionServicios
                .Where(rhs => habitacionesSolicitadas.Contains(rhs.IdHabitacion) &&
                      reservaDetallesDTO.FechaInicio >= rhs.IdReservaNavigation.FechaInicio &&
                      reservaDetallesDTO.FechaFin <= rhs.IdReservaNavigation.FechaFin)
                .AnyAsync();

            if (existe)
            {
                return BadRequest("Una o más habitaciones solicitadas ya están reservadas en las fechas indicadas.");
            }

            var entidad = mapper.Map<Reserva>(reservaDetallesDTO);

            context.Add(entidad);
            await context.SaveChangesAsync();

            foreach (var rhs in reservaDetallesDTO.ReservaHabitacionServicios)
            {
                var nuevaReserva = new ReservaHabitacionServicio
                {
                    IdReserva = entidad.Id,
                    IdHabitacion = rhs.IdHabitacion,
                    IdServicio = rhs.IdServicio
                };

                context.ReservaHabitacionServicios.Add(nuevaReserva);
            }

            await context.SaveChangesAsync();

            var entidadDTO = mapper.Map<ReservaDetallesDTO>(entidad);
            return new CreatedAtRouteResult("obtenerReserva", new { id = entidad.Id }, entidadDTO);
        }
    }
}
