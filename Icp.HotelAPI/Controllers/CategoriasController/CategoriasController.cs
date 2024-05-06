using AutoMapper;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context.Entidades;
using Icp.HotelAPI.Controllers.CategoriasController.DTO;
using Icp.HotelAPI.Controllers.PerfilesController.DTO;
using Icp.HotelAPI.ServiciosCompartidos.AlmacenadorArchivosLocal.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Icp.HotelAPI.Controllers.CategoriasController
{
    [ApiController]
    [Route("api/categorias")]
    public class HabitacionesController : ControllerBase
    {
        private readonly FCT_ABR_11Context context;
        private readonly IMapper mapper;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly string contenedor = "categorias";

        public HabitacionesController(FCT_ABR_11Context context, IMapper mapper, IAlmacenadorArchivos almacenadorArchivos)
        {
            this.context = context;
            this.mapper = mapper;
            this.almacenadorArchivos = almacenadorArchivos;
        }

        // Obtener todas las categorias
        [HttpGet]
        public async Task<ActionResult<List<CategoriaDetallesDTO>>> Get()
        {
            var entidades = await context.Categorias
                .Include(x => x.TipoCamas)
                .ToListAsync();
            var dtos = mapper.Map<List<CategoriaDetallesDTO>>(entidades);
            return dtos;
        }

        // Obtener categoria por {id}
        [HttpGet("{id}", Name = "obtenerCategoria")]
        public async Task<ActionResult<CategoriaDetallesDTO>> Get(int id)
        {
            var entidad = await context.Categorias
                .Include(x => x.TipoCamas)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entidad == null)
            {
                return NotFound();
            }

            var dto = mapper.Map<CategoriaDetallesDTO>(entidad);
            return dto;
        }

        // Introducir una nueva categoria
        [HttpPost]
        public async Task<ActionResult> Post([FromForm] CategoriaCreacionDTO categoriaCreacionDTO)
        {
            var existeTipo = await context.Categorias.AnyAsync(x => x.Tipo == categoriaCreacionDTO.Tipo);

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
            var entidadDTO = mapper.Map<CategoriaDTO>(entidad);
            return new CreatedAtRouteResult("obtenerCategoria", entidadDTO);
        }

        // Cambiar datos categoria por {id}
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

        // Cambiar foto
        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<CategoriaPatchDTO> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var categoriaDB = await context.Categorias.FirstOrDefaultAsync(x => x.Id == id);

            // Verifica si el id de categoria existe
            if (categoriaDB == null)
            {
                return NotFound();
            }

            var categoriaDTO = mapper.Map<CategoriaPatchDTO>(categoriaDB);
            patchDocument.ApplyTo(categoriaDTO, ModelState);

            var esValido = TryValidateModel(categoriaDTO);

            if (!esValido)
            {
                return BadRequest(ModelState);
            }

            mapper.Map(categoriaDTO, categoriaDB);

            await context.SaveChangesAsync();
            return NoContent();
        }

        // Borrar categoria por {id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var entidad = await context.Categorias.FirstOrDefaultAsync(x => x.Id == id);

            if (entidad == null)
            {
                return NotFound();
            }

            // Borra la foto de wwwroot
            await almacenadorArchivos.BorrarArchivo(entidad.Foto, contenedor);
            context.Remove(entidad);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}

