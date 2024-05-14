using AutoMapper;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context;
using Icp.HotelAPI.Controllers.ReservasController.DTO;
using Icp.HotelAPI.Servicios.ReservasService.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Icp.HotelAPI.Controllers.ReservasController
{
    [ApiController]
    [Route("api/reservas")]
    public class ReservasController : CustomBaseController.CustomBaseController
    {
        private readonly FCT_ABR_11Context context;
        private readonly IMapper mapper;
        private readonly IReservaService reservaService;

        public ReservasController(FCT_ABR_11Context context, IMapper mapper, IReservaService reservaService) : base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
            this.reservaService = reservaService;
        }

        //Obtener todas las reservas con sus habitaciones y servicios
        [HttpGet]
        public async Task<ActionResult<List<ReservaDetallesCosteDTO>>> Get()
        {

            var entidades = await context.Reservas
                .Include(x => x.ReservaHabitacionServicios)
                .ToListAsync();

            var dtos = mapper.Map<List<ReservaDetallesCosteDTO>>(entidades);

            return dtos;
        }

        // Obtener reserva con habitaciones y servicios por id reserva
        [HttpGet("{id}", Name = "obtenerReserva")]
        public async Task<ActionResult<ReservaDetallesCosteDTO>> Get(int id)
        {
            var entidad = await context.Reservas
                .Include(x => x.ReservaHabitacionServicios)
                .FirstOrDefaultAsync(x => x.Id == id);
            
            if (entidad == null)
            {
                return NotFound();
            }

            var dto = mapper.Map<ReservaDetallesCosteDTO>(entidad);
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

        // Obtener las reservas con servicios por id servicio
        [HttpGet("servicio/{id}", Name = "obtenerReservaServicio")]
        public async Task<ActionResult<List<ReservaDetallesCosteDTO>>> Get3(int id)
        {
            var entidades = await context.Reservas
                .Include(x => x.ReservaHabitacionServicios)
                .Where(r => r.ReservaHabitacionServicios.Any(rhs => rhs.IdServicio == id))
                .ToListAsync();

            if (entidades == null || entidades.Count == 0)
            {
                return NotFound();
            }

            var dtos = mapper.Map<List<ReservaDetallesCosteDTO>>(entidades);
            return dtos;
        }

        // Agregar reserva con habitaciones y servicios y calcular precio
        [HttpPost]
        public async Task<ActionResult> CrearReserva([FromForm] ReservaCreacionDetallesDTO reservaCreacionDetallesDTO)
        {
            try
            {
                var entidadDTO = await reservaService.CrearReserva(reservaCreacionDetallesDTO);
                return new CreatedAtRouteResult("obtenerReserva", new { id = entidadDTO.Id }, entidadDTO);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Se produjo un error interno");
            }
        }

        // Cambiar datos reserva por id, incluida habitacion y servicios, y recalcular precio
        [HttpPut("{id}")]
        public async Task<ActionResult> ActualizarReserva(int id, [FromForm] ReservaCreacionDetallesDTO reservaCreacionDetallesDTO)
        {
            var actualizado = await reservaService.ActualizarReserva(id, reservaCreacionDetallesDTO);

            if (!actualizado)
            {
                return NotFound();
            }

            return NoContent();
        }

        // Cambiar solamente un campo especifico y recalcular precio
        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<ReservaCreacionDetallesDTO> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var reservaDB = await context.Reservas
                .Include(x => x.ReservaHabitacionServicios)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (reservaDB == null)
            {
                return NotFound();
            }

            var reservaDTO = mapper.Map<ReservaCreacionDetallesDTO>(reservaDB);
            patchDocument.ApplyTo(reservaDTO);

            var actualizado = await reservaService.ActualizarReserva(id, reservaDTO);

            if (!actualizado)
            {
                return NotFound();
            }

            return NoContent();
        }

        // Borrar todos los datos de una reserva
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
