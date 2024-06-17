using AutoMapper;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context.Entidades;
using Icp.HotelAPI.Controllers.PerfilesController.DTO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Icp.HotelAPI.Controllers.PerfilesController
{
    [ApiController]
    [Route("api/perfiles")]
    public class PerfilesController : CustomBaseController.CustomBaseController
    {
        private readonly FCT_ABR_11Context context;
        private readonly IMapper mapper;

        public PerfilesController(
            FCT_ABR_11Context context,
            IMapper mapper)
            : base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        // Obtener los perfiles
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "ADMIN")]
        public async Task<ActionResult<List<PerfilDTO>>> ObtenerPerfiles()
        {
            return await Get<Perfil, PerfilDTO>();
        }

        // Obtener perfil por {id}
        [HttpGet("{id}", Name = "obtenerPerfil")]
        public async Task<ActionResult<PerfilDTO>> ObtenerPerfilesPorId(int id)
        {
            return await Get<Perfil, PerfilDTO>(id);
        }

        // Introducir un nuevo perfil
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "ADMIN")]
        public async Task<ActionResult> CrearNuevoPerfil([FromBody] PerfilCreacionDTO perfilCreacionDTO)
        {
            return await Post<PerfilCreacionDTO, Perfil, PerfilDTO>(perfilCreacionDTO, "obtenerPerfil", "Tipo");
        }

        // Cambiar datos perfil por {id}
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "ADMIN")]
        public async Task<ActionResult> CambiarDatosPerfil(int id, [FromBody] PerfilCreacionDTO perfilCreacionDTO)
        {
            return await Put<PerfilCreacionDTO, Perfil>(perfilCreacionDTO, id);
        }

        // Borrar perfil por {id}
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "ADMIN")]
        public async Task<ActionResult> BorrarPerfil(int id)
        {
            return await Delete<Perfil>(id);
        }
    }
}
