using Icp.HotelAPI.Controllers.UsuariosController.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Icp.HotelAPI.Servicios.UsuariosService.Interfaces
{
    public interface IUsuarioService
    {
        Task<bool> BorrarUsuario(int id);
        Task<bool> ActualizarUsuario(int id, UsuarioCreacionDTO usuarioCreacionDTO);
        Task<RespuestaAutenticacionDTO> Login(UsuarioCredencialesDTO usuarioCredencialesDTO);
        Task<ActionResult<UsuarioDTO>> ObtenerUsuarioPorEmail(UsuarioEmailDTO usuarioEmailDTO);
        Task<ActionResult<UsuarioDTO>> CrearUsuario(UsuarioCreacionDTO usuarioCreacionDTO);
        Task<UsuarioDTO> VerificarDatosUsuario(UsuarioCredencialesDTO usuarioCredencialesDTO);
        Task<bool> BajaUsuario(int id);
        Task<bool> AltaUsuario(int id, UsuarioAltaDTO usuarioAltaDTO);
        Task<bool> AltaTrabajador(int id);
    }
}
