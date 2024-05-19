using AutoMapper;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context;
using Icp.HotelAPI.Controllers.CategoriasController.DTO;
using Icp.HotelAPI.Servicios.CategoriasService.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Icp.HotelAPI.Servicios.CategoriasService
{
    public class CategoriasService : ICategoriaService
    {
        private readonly FCT_ABR_11Context context;
        private readonly IMapper mapper;

        public CategoriasService(FCT_ABR_11Context context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
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
    }
}
