using AutoMapper;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context;
using Microsoft.AspNetCore.Mvc;

namespace Icp.HotelAPI.Controllers.TipoCamasController
{
    [ApiController]
    [Route("api/habitaciones")]
    public class TipoCamasController : ControllerBase
    {
        private readonly FCT_ABR_11Context context;
        private readonly IMapper mapper;

        public TipoCamasController(FCT_ABR_11Context context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
    }
}
