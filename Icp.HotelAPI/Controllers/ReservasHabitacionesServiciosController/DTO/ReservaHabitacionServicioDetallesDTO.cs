namespace Icp.HotelAPI.Controllers.ReservasHabitacionesServiciosController.DTO
{
    public class ReservaHabitacionServicioDetallesDTO : ReservaHabitacionServicoDetallesServicioDTO
    {
        public int IdHabitacion { get; set; }
        public string NombreServicio { get; set; }
        public string TipoHabitacion { get; set; }
    }
}
