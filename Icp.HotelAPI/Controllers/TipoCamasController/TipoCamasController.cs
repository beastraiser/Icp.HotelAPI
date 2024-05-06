using AutoMapper;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context;
using Icp.HotelAPI.Controllers.CategoriasController.DTO;
using Icp.HotelAPI.Controllers.TipoCamasController.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Icp.HotelAPI.Controllers.TipoCamasController
{
    [ApiController]
    [Route("api/tipocama")]
    public class TipoCamasController : ControllerBase
    {
        private readonly FCT_ABR_11Context context;
        private readonly IMapper mapper;

        public TipoCamasController(FCT_ABR_11Context context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        // Obtener todos los tipos de cama
        [HttpGet]
        public async Task<ActionResult<List<TipoCamaDTO>>> Get()
        {
            var entidades = await context.TipoCamas.ToListAsync();
            var dtos = mapper.Map<List<TipoCamaDTO>>(entidades);
            return dtos;
        }
    }

    
}
