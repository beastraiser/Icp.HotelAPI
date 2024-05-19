using AutoMapper;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context.Entidades;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Icp.HotelAPI.Controllers.ServiciosController.DTO;
using Icp.HotelAPI.Servicios.ServiciosService.Interfaces;

namespace Icp.HotelAPI.Controllers.ServiciosController
{
    [ApiController]
    [Route("api/servicios")]
    public class ServiciosController : CustomBaseController.CustomBaseController
    {
        private readonly FCT_ABR_11Context context;
        private readonly IMapper mapper;
        private readonly IServicioService servicioService;

        public ServiciosController(
            FCT_ABR_11Context context, 
            IMapper mapper,
            IServicioService servicioService) 
            : base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
            this.servicioService = servicioService;
        }

        // Obtener todos los registros
        [HttpGet("lista")]
        public async Task<ActionResult<List<ServicioDTO>>> ObtenerTodo()
        {
            return await Get<Servicio, ServicioDTO>();
        }

        // Obtener todos los servicios
        [HttpGet]
        public async Task<ActionResult<List<ServicioDTO>>> ObtenerServicios()
        {
            return await servicioService.ObtenerServicios();
        }

        // Obtener todos los extras
        [HttpGet("extras")]
        public async Task<ActionResult<List<ServicioDTO>>> ObtenerExtras()
        {
            return await servicioService.ObtenerExtras();
        }

        // Obtener servicio por {id}
        [HttpGet("{id}", Name = "obtenerServicio")]
        public async Task<ActionResult<ServicioDTO>> ObtenerServicioPorId(int id)
        {
            return await Get<Servicio, ServicioDTO>(id);
        }

        // Introducir un nuevo servicio
        [HttpPost]
        public async Task<ActionResult> CrearServicio([FromBody] ServicioCreacionDTO servicioCreacionDTO)
        {
            return await Post<ServicioCreacionDTO, Servicio, ServicioDTO>(servicioCreacionDTO, "obtenerServicio", "Nombre");
        }

        // Cambiar datos servicio
        [HttpPut("{id}")]
        public async Task<ActionResult> CambiarDatosServicio(int id, [FromBody] ServicioCreacionDTO servicioCreacionDTO)
        {
            return await Put<ServicioCreacionDTO, Servicio>(servicioCreacionDTO, id);
        }

        // Cambiar un dato especifico
        [HttpPatch("{id}")]
        public async Task<ActionResult> CambiarCampoServicio(int id, JsonPatchDocument<ServicioCreacionDTO> patchDocument)
        {
            return await Patch<Servicio, ServicioCreacionDTO>(id, patchDocument);
        }

        // Borrar servicio
        [HttpDelete("{id}")]
        public async Task<ActionResult> BorrarServicio(int id)
        {
            return await Delete<Servicio>(id);
        }
    }
}
