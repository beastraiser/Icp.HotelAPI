using Icp.HotelAPI.Controllers.Filtros;
using Icp.HotelAPI.Controllers.ReservasHabitacionesServiciosController.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Icp.HotelAPI.Controllers.ReservasController.DTO
{
    public class ReservaModificacionDetallesDTO
    {
        [ModelBinder(BinderType = typeof(TypeBinder<List<ReservaHabitacionServicioDetallesDTO>>))]
        public List<ReservaHabitacionServicioDetallesDTO> ReservaHabitacionServicios { get; set; }

        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
    }
}
