using Icp.HotelAPI.Controllers.ServiciosController.DTO;

namespace Icp.HotelAPI.Servicios.ServiciosService.Interfaces
{
    public interface IServicioService
    {
        Task<List<ServicioDTO>> ObtenerServicios();
        Task<List<ServicioDTO>> ObtenerExtras();
    }
}
