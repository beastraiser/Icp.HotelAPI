using System.ComponentModel.DataAnnotations;

namespace Icp.HotelAPI.Controllers.ReservasController.DTO
{
    public class ReservaCreacionDTO
    {
        public int IdCliente { get; set; }
        public int IdUsuario { get; set; }
        public bool Cancelada { get; set; }
        public bool Pagado { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
    }
}
