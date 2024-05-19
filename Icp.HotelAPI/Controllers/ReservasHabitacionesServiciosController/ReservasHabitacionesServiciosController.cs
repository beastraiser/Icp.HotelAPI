using AutoMapper;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context.Entidades;
using Icp.HotelAPI.Controllers.ReservasHabitacionesServiciosController.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Icp.HotelAPI.Controllers.ReservasHabitacionesServiciosController
{
    [ApiController]
    [Route("api/rhs")]
    public class ReservasHabitacionesServiciosController : CustomBaseController.CustomBaseController
    {
        private readonly FCT_ABR_11Context context;
        private readonly IMapper mapper;

        public ReservasHabitacionesServiciosController(FCT_ABR_11Context context, IMapper mapper) : base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        // Obtener todas las filas
        [HttpGet]
        public async Task<ActionResult<List<ReservaHabitacionServicioDTO>>> ObtenerRHS()
        {
            return await Get<ReservaHabitacionServicio, ReservaHabitacionServicioDTO>();
        }
    }
}
