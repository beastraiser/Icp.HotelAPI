using AutoMapper;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context.Entidades;
using Icp.HotelAPI.Controllers.HabitacionesController.DTO;
using Icp.HotelAPI.Controllers.ReservasController.DTO;
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

        public async Task<bool> ActualizarHabitacion(int id, HabitacionPatchDTO habitacionPatchDTO)
        {
            var entidad = mapper.Map<Habitacion>(habitacionPatchDTO);
            entidad.Id = id;
            context.Entry(entidad).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<HabitacionDTO>> Filtrar(FiltroHabitacionDTO filtroHabitacionDTO)
        {
            var habitacionesQueryable = context.Habitaciones
                .AsQueryable();

            if (filtroHabitacionDTO.IdCategoria != 0)
            {
                habitacionesQueryable = habitacionesQueryable.Where(x => x.IdCategoria == filtroHabitacionDTO.IdCategoria);
            }

            await httpContextAccessor.HttpContext.InsertarParametrosPaginacion(habitacionesQueryable, filtroHabitacionDTO.CantidadRegistrosPorPagina);

            var habitaciones = await habitacionesQueryable.Paginar(filtroHabitacionDTO.Paginacion).ToListAsync();

            return mapper.Map<List<HabitacionDTO>>(habitaciones);
        }

        public async Task<List<HabitacionDetallesDTO>> ObtenerHabitacionesDisponiblesAsync(DisponibilidadRequestDTO disponibilidadRequestDTO)
        {
            var habitaciones = await context.Habitaciones
                .Include(h => h.IdCategoriaNavigation)
                .Where(h => h.IdCategoriaNavigation.MaximoPersonas >= disponibilidadRequestDTO.MaximoPersonas)
                .ToListAsync();

            var habitacionesOcupadas = await context.ReservaHabitacionServicios
            .Where(rhs =>
                    (disponibilidadRequestDTO.FechaInicio.Date == rhs.IdReservaNavigation.FechaInicio.Date &&
                        disponibilidadRequestDTO.FechaFin.Date == rhs.IdReservaNavigation.FechaFin.Date) ||
                    (disponibilidadRequestDTO.FechaInicio >= rhs.IdReservaNavigation.FechaInicio && disponibilidadRequestDTO.FechaInicio < rhs.IdReservaNavigation.FechaFin) ||
                    (disponibilidadRequestDTO.FechaFin <= rhs.IdReservaNavigation.FechaFin && disponibilidadRequestDTO.FechaFin > rhs.IdReservaNavigation.FechaInicio) ||
                    (rhs.IdReservaNavigation.FechaInicio >= disponibilidadRequestDTO.FechaInicio && rhs.IdReservaNavigation.FechaInicio < disponibilidadRequestDTO.FechaFin) ||
                    (rhs.IdReservaNavigation.FechaFin <= disponibilidadRequestDTO.FechaFin && rhs.IdReservaNavigation.FechaFin > disponibilidadRequestDTO.FechaInicio) &&
                    !rhs.IdReservaNavigation.Cancelada)
                .Select(rhs => rhs.IdHabitacion)
                .ToListAsync();

            var habitacionesDisponibles = habitaciones
                .Where(h => !habitacionesOcupadas.Contains(h.Id))
                .ToList();

            return mapper.Map<List<HabitacionDetallesDTO>>(habitacionesDisponibles);
        }
    }
}
