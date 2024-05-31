using Icp.HotelAPI.Controllers.ReservasHabitacionesServiciosController.DTO;

namespace Icp.HotelAPI.Controllers.ReservasController.DTO
{
    public class ReservaDetallesMostrarDTO : ReservaCosteDTO
    {
        public int Id { get; set; }
        public List<ReservaHabitacionServicioDetallesDTO> ReservaHabitacionServicios { get; set; }

        public string NombreCliente { get; set; }
        public string ApellidosCliente { get; set; }
    }
}
