﻿using Icp.HotelAPI.Controllers.ReservasHabitacionesServiciosController.DTO;

namespace Icp.HotelAPI.Controllers.ReservasController.DTO
{
    public class ReservaDetallesServicioDTO : ReservaCosteDTO
    {
        public int Id { get; set; }
        public List<ReservaHabitacionServicoDetallesServicioDTO> ReservaHabitacionServicios { get; set; }
    }
}
