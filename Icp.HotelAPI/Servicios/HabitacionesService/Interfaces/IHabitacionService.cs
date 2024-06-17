using Icp.HotelAPI.Controllers.HabitacionesController.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Icp.HotelAPI.Servicios.HabitacionesService.Interfaces
{
    public interface IHabitacionService
    {
        Task<List<HabitacionDTO>> Filtrar([FromQuery] FiltroHabitacionDTO filtroHabitacionDTO);
        Task<List<HabitacionDetallesDTO>> ObtenerHabitacionesDisponiblesAsync(DisponibilidadRequestDTO disponibilidadRequestDTO);
        Task<bool> ActualizarHabitacion(int id, HabitacionPatchDTO habitacionPatchDTO);
    }
}
