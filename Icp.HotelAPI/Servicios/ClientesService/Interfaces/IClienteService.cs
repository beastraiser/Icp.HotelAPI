using Icp.HotelAPI.Controllers.ClientesController.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Icp.HotelAPI.Servicios.ClientesService.Interfaces
{
    public interface IClienteService
    {
        Task<ActionResult<ClienteDTO>> ObtenerClientePorDni(ClienteDniDTO clienteDniDTO);
        Task<bool> BorrarCliente(int id);
    }
}
