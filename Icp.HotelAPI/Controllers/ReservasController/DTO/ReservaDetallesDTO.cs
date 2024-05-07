using Icp.HotelAPI.Controllers.ReservasHabitacionesServiciosController.DTO;

namespace Icp.HotelAPI.Controllers.ReservasController.DTO
{
    public class ReservaDetallesDTO : ReservaDTO
    {
        public List<ReservaHabitacionServicioDetallesDTO> HabitacionesServicios {  get; set; }
    }
}
