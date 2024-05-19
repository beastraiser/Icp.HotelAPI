using Icp.HotelAPI.Controllers.ClientesUsuariosController.DTO;

namespace Icp.HotelAPI.Servicios.ClientesUsuariosService.Interfaces
{
    public interface IClienteUsuarioService
    {
        Task<bool> Registrar(ClienteUsuarioDTO clienteUsuarioDTO);
        Task<VClienteUsuarioDetallesUsuarioDTO> ObtenerClienteUsuarioPorIdCliente(int id);
        Task<VClienteUsuarioDetallesClienteDTO> ObtenerClienteUsuarioPorIdUsuario(int id);
    }
}
