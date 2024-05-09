using Icp.HotelAPI.Controllers.ReservasHabitacionesServiciosController.DTO;

namespace Icp.HotelAPI.Controllers.ReservasController.DTO
{
    public class ReservaDetallesServicioDTO : ReservaCreacionDTO
    {
        public int Id { get; set; }
        public List<ReservaHabitacionServicoDetallesServicioDTO> ReservaHabitacionServicios { get; set; }
    }
}
