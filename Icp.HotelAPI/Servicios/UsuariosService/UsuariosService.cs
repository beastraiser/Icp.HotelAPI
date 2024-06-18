using AutoMapper;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context.Entidades;
using Icp.HotelAPI.Controllers.Interfaces;
using Icp.HotelAPI.Controllers.UsuariosController.DTO;
using Icp.HotelAPI.Servicios.UsuariosService.Interfaces;
using Icp.HotelAPI.ServiciosCompartidos.LoginService;
using Icp.HotelAPI.ServiciosCompartidos.LoginService.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
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


        public async Task<ActionResult<UsuarioDTO>> CrearUsuario(UsuarioCreacionDTO usuarioCreacionDTO)
        {
            var existe = await context.Usuarios.AnyAsync(e => e.Email == usuarioCreacionDTO.Email);
            if (existe)
            {
                throw new InvalidOperationException("Ya existe un usuario con ese email");
            }

            var entidad = mapper.Map<Usuario>(usuarioCreacionDTO);
            entidad.Contrasenya = loginService.HashContrasenya(entidad.Contrasenya);
            context.Add(entidad);
            await context.SaveChangesAsync();
            var dtoLectura = mapper.Map<UsuarioDTO>(entidad);
            return new CreatedAtRouteResult("obtenerUsuario", dtoLectura);
        }

        public async Task<bool> ActualizarUsuario(int id, UsuarioCreacionDTO usuarioCreacionDTO)
        {
            usuarioCreacionDTO.Contrasenya = loginService.HashContrasenya(usuarioCreacionDTO.Contrasenya);

            var entidad = mapper.Map<Usuario>(usuarioCreacionDTO);

            entidad.Id = id;

            context.Entry(entidad).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return true;
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

        public async Task<bool> BajaUsuario(int id)
        {
            var entidad = await context.Usuarios.FirstOrDefaultAsync(x => x.Id == id);

            if (entidad == null)
            {
                return false;
            }

            if (entidad.IdPerfil == 4)
            {
                var clienteUsuario = await context.ClienteUsuarios
               .Where(tc => tc.IdUsuario == id)
               .ToListAsync();

                context.ClienteUsuarios.RemoveRange(clienteUsuario);
            }

            entidad.Baja = true;

            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AltaUsuario(int id, UsuarioAltaDTO usuarioAltaDTO)
        {
            var entidad = await context.Usuarios.FirstOrDefaultAsync(x => x.Id == id);

            if (entidad == null)
            {
                throw new InvalidOperationException("Usuario no encontrado");
            }

            var cliente = await context.Clientes.FirstOrDefaultAsync(c => c.Dni == usuarioAltaDTO.Dni);
            if (cliente == null)
            {
                throw new InvalidOperationException("Cliente no encontrado");
            }

            var existeClienteUsuario = await context.ClienteUsuarios.FirstOrDefaultAsync(c => c.IdCliente == cliente.Id);

            if (existeClienteUsuario != null)
            {
                throw new InvalidOperationException("El cliente ya tiene una cuenta asociada");
            }

            entidad.Baja = false;

            var clienteUsuario = new ClienteUsuario
            {
                IdCliente = cliente.Id,
                IdUsuario = id
            };

            context.ClienteUsuarios.Add(clienteUsuario);

            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AltaTrabajador(int id)
        {
            var entidad = await context.Usuarios.FirstOrDefaultAsync(x => x.Id == id);

            if (entidad == null)
            {
                throw new InvalidOperationException("Usuario no encontrado");
            }

            entidad.Baja = false;

            await context.SaveChangesAsync();
            return true;
        }

        public async Task<RespuestaAutenticacionDTO> Login(UsuarioCredencialesDTO usuarioCredencialesDTO)
        {
            var resultado = await context.Usuarios.FirstOrDefaultAsync(x => x.Email == usuarioCredencialesDTO.Email);

            if (resultado == null)
            {
                throw new InvalidOperationException("Usuario no encontrado");
            }

            if (resultado.Baja)
            {
                throw new InvalidOperationException("Usuario dado de baja");
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

        public async Task<UsuarioDTO> VerificarDatosUsuario(UsuarioCredencialesDTO usuarioCredencialesDTO)
        {
            var resultado = await context.Usuarios.FirstOrDefaultAsync(x => x.Email == usuarioCredencialesDTO.Email);

            if (resultado == null)
            {
                throw new InvalidOperationException("Usuario no encontrado");
            }

            if (resultado.Baja)
            {
                throw new InvalidOperationException("Usuario dado de baja");
            }

            if (!loginService.VerificarContrasenya(usuarioCredencialesDTO.Contrasenya, resultado.Contrasenya))
            {
                throw new InvalidOperationException("Contraseña incorrecta");
            }

            return mapper.Map<UsuarioDTO>(resultado); 
        }
    }
}
