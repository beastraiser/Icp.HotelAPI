using Icp.HotelAPI.Controllers.ReservasHabitacionesServiciosController.DTO;

namespace Icp.HotelAPI.Controllers.ReservasController.DTO
{
    public class ReservaDetallesDTO : ReservaCreacionDTO
    {
        public List<ReservaHabitacionServicioDetallesDTO> ReservaHabitacionServicios {  get; set; }
    }
}
