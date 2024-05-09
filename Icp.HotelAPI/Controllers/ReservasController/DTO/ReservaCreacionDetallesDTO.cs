using Icp.HotelAPI.Controllers.Filtros;
using Icp.HotelAPI.Controllers.ReservasHabitacionesServiciosController.DTO;
using Icp.HotelAPI.Controllers.TipoCamasController.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Icp.HotelAPI.Controllers.ReservasController.DTO
{
    public class ReservaCreacionDetallesDTO : ReservaCreacionDTO
    {
        [ModelBinder(BinderType = typeof(TypeBinder<List<ReservaHabitacionServicioDetallesDTO>>))]
        public List<ReservaHabitacionServicioDetallesDTO> ReservaHabitacionServicios { get; set; }
    }
}
