using Icp.HotelAPI.Controllers.ReservasController.DTO;

namespace Icp.HotelAPI.Servicios.ReservasService.Interfaces
{
    public interface IReservaService
    {
        Task<ReservaDetallesCosteDTO> CrearReserva(ReservaCreacionDetallesDTO reservaCreacionDetallesDTO);
        Task<bool> ActualizarReserva(int id, ReservaCreacionDetallesDTO reservaCreacionDetallesDTO);
        Task<bool> CancelarReserva(int id);
    }
}
