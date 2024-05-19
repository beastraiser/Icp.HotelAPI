﻿using Icp.HotelAPI.Controllers.ReservasController.DTO;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Icp.HotelAPI.Servicios.ReservasService.Interfaces
{
    public interface IReservaService
    {
        Task<List<ReservaDetallesCosteDTO>> ObtenerReservas();
        Task<ReservaDetallesCosteDTO> ObtenerReservasPorId(int id);
        Task<List<ReservaDetallesServicioDTO>> ObtenerReservasPorIdHabitacion(int id);
        Task<List<ReservaDetallesCosteDTO>> ObtenerReservasPorIdServicio(int id);
        Task<ActionResult> CrearReserva(ReservaCreacionDetallesDTO reservaCreacionDetallesDTO);
        Task<bool> ActualizarReserva(int id, ReservaCreacionDetallesDTO reservaCreacionDetallesDTO);
        Task<bool> CancelarReserva(int id);
        Task<bool> CambiarCampoReserva(int id, [FromBody] JsonPatchDocument<ReservaCreacionDetallesDTO> patchDocument);
        Task<bool> BorrarReserva(int id);
    }
}
