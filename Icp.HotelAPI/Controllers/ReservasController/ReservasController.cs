using AutoMapper;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context;
using Icp.HotelAPI.Controllers.CategoriasController.DTO;
using Icp.HotelAPI.Controllers.ReservasController.DTO;
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
    }
}
