namespace Icp.HotelAPI.Controllers.HabitacionesController.DTO
{
    public class HabitacionDetallesDTO
    {
        public int Id { get; set; }
        public int IdCategoria { get; set; }
        public string CategoriaTipo { get; set; }
        public int NumeroCamas { get; set; }
        public int MaximoPersonas { get; set; }
        public decimal CosteNoche { get; set; }
    }
}
