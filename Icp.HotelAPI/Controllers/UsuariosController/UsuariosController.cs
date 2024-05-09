using AutoMapper;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context.Entidades;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context;
using Icp.HotelAPI.Controllers.ReservasController.DTO;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Icp.HotelAPI.Controllers.UsuariosController
{
    [ApiController]
    [Route("api/reservas")]
    public class UsuariosController : CustomBaseController.CustomBaseController
    {
        private readonly FCT_ABR_11Context context;
        private readonly IMapper mapper;

        public UsuariosController(FCT_ABR_11Context context, IMapper mapper) : base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        //Obtener todos los usuarios con sus clientes asociados
        [HttpGet]
        public async Task<ActionResult<List<ReservaDetallesDTO>>> Get()
        {

            var entidades = await context.Reservas
                .Include(x => x.ReservaHabitacionServicios)
                .ToListAsync();

            var dtos = mapper.Map<List<ReservaDetallesDTO>>(entidades);

            return dtos;
        }

        // Obtener reserva con habitaciones y servicios por id reserva
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

        // Obtener las reservas con servicios por id habitacion
        [HttpGet("habitacion/{id}", Name = "obtenerReservaHabitacion")]
        public async Task<ActionResult<List<ReservaDetallesServicioDTO>>> Get2(int id)
        {
            var entidades = await context.Reservas
                .Include(x => x.ReservaHabitacionServicios)
                .Where(r => r.ReservaHabitacionServicios.Any(rhs => rhs.IdHabitacion == id))
                .ToListAsync();

            if (entidades == null || entidades.Count == 0)
            {
                return NotFound();
            }

            var dtos = mapper.Map<List<ReservaDetallesServicioDTO>>(entidades);
            return dtos;
        }

        // Agregar reserva con habitaciones y servicios
        [HttpPost]
        public async Task<ActionResult> Post([FromForm] ReservaCreacionDetallesDTO reservaCreacionDetallesDTO)
        {
            var habitacionesSolicitadas = reservaCreacionDetallesDTO.ReservaHabitacionServicios.Select(hs => hs.IdHabitacion).ToList();

            var existe = await context.ReservaHabitacionServicios
                .Where(rhs => habitacionesSolicitadas.Contains(rhs.IdHabitacion) &&
                      reservaCreacionDetallesDTO.FechaInicio >= rhs.IdReservaNavigation.FechaInicio &&
                      reservaCreacionDetallesDTO.FechaInicio < rhs.IdReservaNavigation.FechaFin ||
                      reservaCreacionDetallesDTO.FechaFin <= rhs.IdReservaNavigation.FechaFin &&
                      reservaCreacionDetallesDTO.FechaFin > rhs.IdReservaNavigation.FechaInicio ||
                      rhs.IdReservaNavigation.FechaInicio >= reservaCreacionDetallesDTO.FechaInicio &&
                      rhs.IdReservaNavigation.FechaInicio < reservaCreacionDetallesDTO.FechaFin ||
                      rhs.IdReservaNavigation.FechaFin <= reservaCreacionDetallesDTO.FechaFin &&
                      rhs.IdReservaNavigation.FechaFin > reservaCreacionDetallesDTO.FechaInicio
                      )
                .AnyAsync();

            if (existe)
            {
                return BadRequest("Una o más habitaciones solicitadas ya están reservadas en las fechas indicadas.");
            }

            var entidad = mapper.Map<Reserva>(reservaCreacionDetallesDTO);

            context.Add(entidad);
            await context.SaveChangesAsync();

            foreach (var rhs in reservaCreacionDetallesDTO.ReservaHabitacionServicios)
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

        // Cambiar datos reserva por id, incluida habitacion y servicios
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromForm] ReservaCreacionDetallesDTO reservaCreacionDetallesDTO)
        {
            var reservaDB = await context.Reservas
                .Include(x => x.ReservaHabitacionServicios)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (reservaDB == null)
            {
                return NotFound();
            }

            // Al mapearlo de esta manera solo se actualizan aquellos campos que son distintos
            reservaDB = mapper.Map(reservaCreacionDetallesDTO, reservaDB);

            reservaDB.ReservaHabitacionServicios.Clear();

            foreach (var habitacionDTO in reservaCreacionDetallesDTO.ReservaHabitacionServicios)
            {
                var habitacion = new ReservaHabitacionServicio
                {
                    IdReserva = id,
                    IdHabitacion = habitacionDTO.IdHabitacion,
                    IdServicio = habitacionDTO.IdServicio
                };
                reservaDB.ReservaHabitacionServicios.Add(habitacion);
            }

            context.Entry(reservaDB).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }

        // Cambiar solamente un campo especifico
        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<ReservaCreacionDetallesDTO> patchDocument)
        {
            var reservaDB = await context.Reservas
                .Include(x => x.ReservaHabitacionServicios)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (reservaDB == null)
            {
                return NotFound();
            }

            var reservaDTO = mapper.Map<ReservaCreacionDetallesDTO>(reservaDB);

            patchDocument.ApplyTo(reservaDTO);

            // Actualizar las habitaciones asociadas a la reserva
            reservaDB.ReservaHabitacionServicios.Clear(); // Eliminar todas las habitaciones asociadas actualmente

            // Agregar las nuevas habitaciones asociadas
            foreach (var habitacionDTO in reservaDTO.ReservaHabitacionServicios)
            {
                var habitacion = new ReservaHabitacionServicio
                {
                    IdReserva = id,
                    IdHabitacion = habitacionDTO.IdHabitacion,
                    IdServicio = habitacionDTO.IdServicio
                };
                reservaDB.ReservaHabitacionServicios.Add(habitacion);
            }

            await context.SaveChangesAsync();

            return NoContent();
        }


        // Borrar datos de una reserva
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var entidad = await context.Reservas.FirstOrDefaultAsync(x => x.Id == id);

            if (entidad == null)
            {
                return NotFound();
            }

            var reservaHabitacionServicio = await context.ReservaHabitacionServicios
                .Where(tc => tc.IdReserva == id)
                .ToListAsync();

            context.ReservaHabitacionServicios.RemoveRange(reservaHabitacionServicio);
            context.Reservas.Remove(entidad);

            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
