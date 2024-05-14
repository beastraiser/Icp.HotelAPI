using AutoMapper;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context.Entidades;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context;
using Icp.HotelAPI.Controllers.ClientesController.DTO;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Icp.HotelAPI.Controllers.ServiciosController.DTO;

namespace Icp.HotelAPI.Controllers.ServiciosController
{
    [ApiController]
    [Route("api/servicios")]
    public class ServiciosController : CustomBaseController.CustomBaseController
    {
        private readonly FCT_ABR_11Context context;
        private readonly IMapper mapper;

        public ServiciosController(FCT_ABR_11Context context, IMapper mapper) : base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        // Obtener todos los servicios
        [HttpGet]
        public async Task<ActionResult<List<ServicioDTO>>> Get()
        {
            return await Get<Servicio, ServicioDTO>();
        }

        // Obtener servicio por {id}
        [HttpGet("{id}", Name = "obtenerServicio")]
        public async Task<ActionResult<ServicioDTO>> Get(int id)
        {
            return await Get<Servicio, ServicioDTO>(id);
        }

        // Introducir un nuevo servicio
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ServicioCreacionDTO servicioCreacionDTO, int id)
        {
            return await Post<ServicioCreacionDTO, Servicio, ServicioDTO>(servicioCreacionDTO, "obtenerServicio", id);
        }

        // Cambiar datos servicio
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] ServicioCreacionDTO servicioCreacionDTO)
        {
            return await Put<ServicioCreacionDTO, Servicio>(servicioCreacionDTO, id);
        }

        // Cambiar un dato especifico
        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<ServicioCreacionDTO> patchDocument)
        {
            return await Patch<Servicio, ServicioCreacionDTO>(id, patchDocument);
        }

        // Borrar cliente
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            return await Delete<Servicio>(id);
        }
    }
}
