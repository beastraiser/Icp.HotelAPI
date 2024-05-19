using AutoMapper;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context.Entidades;
using Icp.HotelAPI.Controllers.CategoriasController.DTO;
using Icp.HotelAPI.Servicios.CategoriasService.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Icp.HotelAPI.Controllers.CategoriasController
{
    [ApiController]
    [Route("api/categorias")]

    public class CategoriasController : CustomBaseController.CustomBaseController
    {
        private readonly FCT_ABR_11Context context;
        private readonly IMapper mapper;
        private readonly ICategoriaService categoriaService;

        public CategoriasController(
            FCT_ABR_11Context context, 
            IMapper mapper, 
            ICategoriaService categoriaService) 
            : base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
            this.categoriaService = categoriaService;
        }

        // Obtener todas las categorias con tipo cama
        [HttpGet]
        public async Task<ActionResult<List<CategoriaDetallesDTO>>> ObtenerCategorias()
        {
            return await categoriaService.ObtenerCategorias();
        }

        // Obtener categoria por {id} con tipo cama
        [HttpGet("{id}", Name = "obtenerCategoria")]
        public async Task<ActionResult<CategoriaDetallesDTO>> ObtenerCategoriaId(int id)
        {
            return await categoriaService.ObtenerCategoriaId(id);
        }

        // Introducir una nueva categoria con tipos de cama
        [HttpPost]
        public async Task<ActionResult> NuevaCategoria([FromForm] CategoriaCreacionDTO categoriaCreacionDTO)
        {
            return await categoriaService.NuevaCategoria(categoriaCreacionDTO);
        }

        // Cambiar foto
        [HttpPut("{id}")]
        public async Task<ActionResult> CambiarDatosCategoria(int id, [FromForm] CategoriaCreacionDTO categoriaCreacionDTO)
        {
            var exito = await categoriaService.CambiarDatosCategoria(id, categoriaCreacionDTO);

            if (!exito)
            {
                return NotFound();
            }
            else
            {
                return NoContent();
            }
        }

        // Cambiar datos categoria por {id}
        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<CategoriaPatchDTO> patchDocument)
        {
            return await Patch<Categoria, CategoriaPatchDTO>(id, patchDocument);
        }

        // Borrar categoria y tipos de cama por {id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> BorrarCategoria(int id)
        {
            var exito = await categoriaService.BorrarCategoria(id);

            if (!exito)
            {
                return NotFound();
            }
            else
            {
                return NoContent();
            }
        }
    }
}

