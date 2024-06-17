using Icp.HotelAPI.Controllers.UsuariosController.DTO;
using System.Security.Claims;

namespace Icp.HotelAPI.ServiciosCompartidos.LoginService.Interfaces
{
    public interface ILoginService
    {
        RespuestaAutenticacionDTO ConstruirToken(List<Claim> claims);
        public string HashContrasenya(string contrasenya);
        public bool VerificarContrasenya(string contrasenya, string hash);
        
    }
}
