using AutoMapper;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context;
using Icp.HotelAPI.Controllers.Interfaces;
using Icp.HotelAPI.Controllers.UsuariosController.DTO;
using Icp.HotelAPI.Servicios.UsuariosService.Interfaces;
using Icp.HotelAPI.ServiciosCompartidos.LoginService;
using Icp.HotelAPI.ServiciosCompartidos.LoginService.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Icp.HotelAPI.Servicios.UsuariosService
{
    public class UsuariosService : IUsuarioService
    {
        private readonly FCT_ABR_11Context context;
        private readonly IMapper mapper;
        private readonly ILoginService loginService;

        public UsuariosService(
            FCT_ABR_11Context context,
            IMapper mapper,
            ILoginService loginService)
        {
            this.context = context;
            this.mapper = mapper;
            this.loginService = loginService;
        }

        public async Task<ActionResult<UsuarioDTO>> ObtenerUsuarioPorEmail(UsuarioEmailDTO usuarioEmailDTO)
        {
            var entidad = await context.Usuarios.FirstOrDefaultAsync(x => x.Email == usuarioEmailDTO.Email);
            if (entidad == null)
            {
                throw new InvalidOperationException("Usuario no encontrado");
            }
            return mapper.Map<UsuarioDTO>(entidad);
        }

        public async Task<bool> BorrarUsuario(int id)
        {
            var entidad = await context.Usuarios.FirstOrDefaultAsync(x => x.Id == id);

            if (entidad == null)
            {
                return false;
            }

            var clienteUsuario = await context.ClienteUsuarios
                .Where(tc => tc.IdUsuario == id)
                .ToListAsync();

            context.ClienteUsuarios.RemoveRange(clienteUsuario);
            context.Usuarios.Remove(entidad);

            await context.SaveChangesAsync();
            return true;
        }

        public async Task<RespuestaAutenticacionDTO> Login(UsuarioCredencialesDTO usuarioCredencialesDTO)
        {
            var resultado = await context.Usuarios.FirstOrDefaultAsync(x => x.Email == usuarioCredencialesDTO.Email);

            // Vuelvo a hashear la contraseña para evitar errores por inserviones a mano desde SQL
            //resultado.Contrasenya = loginService.HashContrasenya(usuarioCredencialesDTO.Contrasenya);
            //await context.SaveChangesAsync();


            if (resultado == null)
            {
                throw new InvalidOperationException("Usuario no encontrado");
            }

            if (!loginService.VerificarContrasenya(usuarioCredencialesDTO.Contrasenya, resultado.Contrasenya))
            {
                throw new InvalidOperationException("Contraseña incorrecta");
            }

            var claims = new List<Claim>()
            {
                new Claim("email", usuarioCredencialesDTO.Email),
                new Claim("id", resultado.Id.ToString())
            };

            switch (resultado.IdPerfil)
            {
                case 1:
                    var adminClaim = new Claim("rol", "ADMIN");
                    claims.Add(adminClaim);
                    break;
                case 2:
                    var recepcionClaim = new Claim("rol", "RECEPCION");
                    claims.Add(recepcionClaim);
                    break;
                case 4:
                    var clienteClaim = new Claim("rol", "CLIENTE");
                    claims.Add(clienteClaim);
                    break;
            }

            var respuestaAutenticacion = loginService.ConstruirToken(claims);
            return respuestaAutenticacion;
        }
    }
}
