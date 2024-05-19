using AutoMapper;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context;
using Icp.HotelAPI.Controllers.HabitacionesController.DTO;
using Icp.HotelAPI.Servicios.HabitacionesService.Interfaces;
using Icp.HotelAPI.ServiciosCompartidos.PaginacionDTO.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Icp.HotelAPI.Servicios.HabitacionesService
{
    public class HabitacionesService : IHabitacionService
    {
        private readonly FCT_ABR_11Context context;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;

        public HabitacionesService(
            FCT_ABR_11Context context,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<HabitacionDTO>> Filtrar(FiltroHabitacionDTO filtroHabitacionDTO)
        {
            var habitacionesQueryable = context.Habitaciones
                .Where(x => x.Disponibilidad == true)
                .AsQueryable();

            if (filtroHabitacionDTO.IdCategoria != 0)
            {
                habitacionesQueryable = habitacionesQueryable.Where(x => x.IdCategoria == filtroHabitacionDTO.IdCategoria);
            }

            await httpContextAccessor.HttpContext.InsertarParametrosPaginacion(habitacionesQueryable, filtroHabitacionDTO.CantidadRegistrosPorPagina);

            var habitaciones = await habitacionesQueryable.Paginar(filtroHabitacionDTO.Paginacion).ToListAsync();

            return mapper.Map<List<HabitacionDTO>>(habitaciones);
        }
    }
}
