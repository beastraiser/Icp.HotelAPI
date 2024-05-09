using System.ComponentModel.DataAnnotations;

namespace Icp.HotelAPI.Controllers.ReservasController.DTO
{
    public class ReservaCreacionDTO
    {
        public int IdCliente { get; set; }
        public int IdUsuario { get; set; }

        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        //[DataType(DataType.Date)]
        public DateTime FechaInicio { get; set; }

        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        //[DataType(DataType.Date)]
        public DateTime FechaFin { get; set; }

        public decimal CosteTotal { get; set; }
    }
}
