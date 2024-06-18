using AutoMapper;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context.Entidades;
using Icp.HotelAPI.Controllers.CategoriasController.DTO;
using Icp.HotelAPI.Servicios.CategoriasService.Interfaces;
using Icp.HotelAPI.ServiciosCompartidos.AlmacenadorArchivosLocal.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Icp.HotelAPI.Servicios.CategoriasService
{
    public class CategoriasService : ICategoriaService
    {
        private readonly FCT_ABR_11Context context;
        private readonly IMapper mapper;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly string contenedor = "categorias";

        public CategoriasService(
            FCT_ABR_11Context context,
            IMapper mapper,
            IAlmacenadorArchivos almacenadorArchivos)
        {
            this.context = context;
            this.mapper = mapper;
            this.almacenadorArchivos = almacenadorArchivos;
        }

        public async Task<List<CategoriaDetallesDTO>> ObtenerCategorias()
        {
            var entidades = await context.Categorias
                .Include(x => x.TipoCamas)
                .ToListAsync();
            var dtos = mapper.Map<List<CategoriaDetallesDTO>>(entidades);
            return dtos;
        }

        public async Task<CategoriaDetallesDTO> ObtenerCategoriaId(int id)
        {
            var entidad = await context.Categorias
                .Include(x => x.TipoCamas)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entidad == null)
            {
                throw new InvalidOperationException("La categoria indicada no existe");
            }

            var dto = mapper.Map<CategoriaDetallesDTO>(entidad);
            return dto;
        }

        public async Task<ActionResult> NuevaCategoria([FromForm] CategoriaCreacionDTO categoriaCreacionDTO)
        {
            var existeTipo = await context.Categorias
                .AnyAsync(x => x.Tipo == categoriaCreacionDTO.Tipo);

            if (existeTipo)
            {
                throw new InvalidOperationException($"Ya existe una categoría {categoriaCreacionDTO.Tipo}");
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

        public async Task<bool> CambiarDatosCategoria(int id, [FromForm] CategoriaCreacionDTO categoriaCreacionDTO)
        {
            var categoriaDB = await context.Categorias
                                  .Include(c => c.TipoCamas)
                                  .FirstOrDefaultAsync(x => x.Id == id);

            if (categoriaDB == null)
            {
                return false;
            }

            // Actualizar propiedades de categoriaDB con los valores de categoriaCreacionDTO
            categoriaDB = mapper.Map(categoriaCreacionDTO, categoriaDB);

            // Actualizar la colección TipoCamas
            categoriaDB.TipoCamas.Clear();
            foreach (var tipoCamaDTO in categoriaCreacionDTO.TipoCamas)
            {
                categoriaDB.TipoCamas.Add(new TipoCama { Tipo = tipoCamaDTO.Tipo });
            }

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
            return true;
        }

        public async Task<bool> BorrarCategoria(int id)
        {
            var entidad = await context.Categorias.FirstOrDefaultAsync(x => x.Id == id);

            if (entidad == null)
            {
                return false;
            }

            var tipoCamas = await context.TipoCamas
                .Where(tc => tc.IdCategoria == id)
                .ToListAsync();

            // Borra la foto de wwwroot
            await almacenadorArchivos.BorrarArchivo(entidad.Foto, contenedor);

            context.TipoCamas.RemoveRange(tipoCamas);
            context.Categorias.Remove(entidad);

            await context.SaveChangesAsync();
            return true;
        }
    }
}
