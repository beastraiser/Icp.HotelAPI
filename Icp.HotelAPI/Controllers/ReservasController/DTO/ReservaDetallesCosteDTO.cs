using Icp.HotelAPI.Controllers.ReservasHabitacionesServiciosController.DTO;

namespace Icp.HotelAPI.Controllers.ReservasController.DTO
{
    public class ReservaDetallesCosteDTO : ReservaCosteDTO
    {
        public int Id { get; set; }
        public List<ReservaHabitacionServicioDetallesDTO> ReservaHabitacionServicios { get; set; }
    }
}
