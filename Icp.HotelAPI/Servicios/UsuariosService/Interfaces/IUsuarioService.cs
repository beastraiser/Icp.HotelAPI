using Icp.HotelAPI.Controllers.UsuariosController.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Icp.HotelAPI.Servicios.UsuariosService.Interfaces
{
    public interface IUsuarioService
    {
        Task<bool> BorrarUsuario(int id);
        Task<RespuestaAutenticacionDTO> Login(UsuarioCredencialesDTO usuarioCredencialesDTO);
        Task<ActionResult<UsuarioDTO>> ObtenerUsuarioPorEmail(UsuarioEmailDTO usuarioEmailDTO);
        Task<ActionResult<UsuarioDTO>> CrearUsuario(UsuarioCreacionDTO usuarioCreacionDTO);
    }
}
