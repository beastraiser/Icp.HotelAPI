using AutoMapper;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context.Entidades;
using Icp.HotelAPI.Controllers.CategoriasController.DTO;
using Icp.HotelAPI.Servicios.CategoriasService.Interfaces;
using Icp.HotelAPI.ServiciosCompartidos.AlmacenadorArchivosLocal.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Icp.HotelAPI.Controllers.CategoriasController
{
    [ApiController]
    [Route("api/categorias")]

    public class CategoriasController : CustomBaseController.CustomBaseController
    {
        private readonly FCT_ABR_11Context context;
        private readonly IMapper mapper;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly ICategoriaService categoriaService;
        private readonly string contenedor = "categorias";

        public CategoriasController(
            FCT_ABR_11Context context, 
            IMapper mapper, 
            IAlmacenadorArchivos almacenadorArchivos,
            ICategoriaService categoriaService) 
            : base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
            this.almacenadorArchivos = almacenadorArchivos;
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
        public async Task<ActionResult> Post([FromForm] CategoriaCreacionDTO categoriaCreacionDTO)
        {
            var existeTipo = await context.Categorias
                .AnyAsync(x => x.Tipo == categoriaCreacionDTO.Tipo);

            if (existeTipo)
            {
                return BadRequest($"Ya existe una categoría {categoriaCreacionDTO.Tipo}");
            }

            var entidad = mapper.Map<Categoria>(categoriaCreacionDTO);

            // Lógica para subir una foto
            if (categoriaCreacionDTO.Foto != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await categoriaCreacionDTO.Foto.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(categoriaCreacionDTO.Foto.FileName);
                    entidad.Foto = await almacenadorArchivos.GuardarArchivo(contenido, extension, contenedor, categoriaCreacionDTO.Foto.ContentType);
                }
            }

            context.Add(entidad);
            await context.SaveChangesAsync();

            foreach (var tipoCamaDTO in categoriaCreacionDTO.TipoCamas)
            {
                var tipoCama = new TipoCama
                {
                    IdCategoria = entidad.Id,
                    Tipo = tipoCamaDTO.Tipo
                };

                context.TipoCamas.Add(tipoCama);
            }

            await context.SaveChangesAsync();

            var entidadDTO = mapper.Map<CategoriaDetallesDTO>(entidad);
            return new CreatedAtRouteResult("obtenerCategoria", new { id = entidad.Id }, entidadDTO);
        }

        // Cambiar foto
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromForm] CategoriaCreacionDTO categoriaCreacionDTO)
        {
            var categoriaDB = await context.Categorias.FirstOrDefaultAsync(x => x.Id == id);

            if (categoriaDB == null)
            {
                return NotFound();
            }

            // Al mapearlo de esta manera solo se actualizan aquellos campos que son distintos
            categoriaDB = mapper.Map(categoriaCreacionDTO, categoriaDB);

            // Lógica para editar una foto
            if (categoriaCreacionDTO.Foto != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await categoriaCreacionDTO.Foto.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(categoriaCreacionDTO.Foto.FileName);
                    categoriaDB.Foto = await almacenadorArchivos.EditarArchivo(contenido, extension, contenedor, categoriaDB.Foto, categoriaCreacionDTO.Foto.ContentType);
                }
            }

            await context.SaveChangesAsync();
            return NoContent();
        }

        // Cambiar datos categoria por {id}
        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<CategoriaPatchDTO> patchDocument)
        {
            return await Patch<Categoria, CategoriaPatchDTO>(id, patchDocument);
        }

        // Borrar categoria y tipos de cama por {id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var entidad = await context.Categorias.FirstOrDefaultAsync(x => x.Id == id);

            if (entidad == null)
            {
                return NotFound();
            }

            var tipoCamas = await context.TipoCamas
                .Where(tc => tc.IdCategoria == id)
                .ToListAsync();

            // Borra la foto de wwwroot
            await almacenadorArchivos.BorrarArchivo(entidad.Foto, contenedor);

            context.TipoCamas.RemoveRange(tipoCamas);
            context.Categorias.Remove(entidad);

            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}

