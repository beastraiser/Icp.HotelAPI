using AutoMapper;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context.Entidades;
using Icp.HotelAPI.Controllers.TipoCamasController.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Icp.HotelAPI.Controllers.TipoCamasController
{
    [ApiController]
    [Route("api/tipocama")]
    public class TipoCamasController : CustomBaseController.CustomBaseController
    {
        private readonly FCT_ABR_11Context context;
        private readonly IMapper mapper;

        public TipoCamasController(
            FCT_ABR_11Context context, 
            IMapper mapper)
            : base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        // Obtener todos los tipos de cama
        [HttpGet]
        public async Task<ActionResult<List<TipoCamaDTO>>> ObtenerTodo()
        {
            return await Get<TipoCama, TipoCamaDTO>();
        }
    }

    
}
