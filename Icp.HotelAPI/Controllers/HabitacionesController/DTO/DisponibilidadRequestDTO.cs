namespace Icp.HotelAPI.Controllers.HabitacionesController.DTO
{
    public class DisponibilidadRequestDTO
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int MaximoPersonas { get; set; }
    }
}
