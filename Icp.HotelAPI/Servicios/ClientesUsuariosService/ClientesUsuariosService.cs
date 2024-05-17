using AutoMapper;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context.Entidades;
using Icp.HotelAPI.Controllers.ClientesUsuariosController.DTO;
using Icp.HotelAPI.Controllers.UsuariosController.DTO;
using Icp.HotelAPI.Servicios.ClientesUsuariosService.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Icp.HotelAPI.Servicios.ClientesUsuariosService
{
    public class ClientesUsuariosService : IClienteUsuarioService
    {
        private readonly FCT_ABR_11Context context;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;

        public ClientesUsuariosService(FCT_ABR_11Context context,
            IMapper mapper,
            IConfiguration configuration)
        {
            this.context = context;
            this.mapper = mapper;
            this.configuration = configuration;
        }

        public async Task<bool> Registrar(ClienteUsuarioDTO clienteUsuarioDTO)
        {
            try
            {
                // Crear el cliente
                var cliente = mapper.Map<Cliente>(clienteUsuarioDTO);
                context.Clientes.Add(cliente);
                await context.SaveChangesAsync();

                // Crear el usuario
                var usuario = mapper.Map<Usuario>(clienteUsuarioDTO);
                usuario.Contrasenya = HashContrasenya(usuario.Contrasenya);
                context.Usuarios.Add(usuario);
                await context.SaveChangesAsync();

                // Asociar el cliente y el usuario
                var clienteUsuario = new ClienteUsuario { IdCliente = cliente.Id, IdUsuario = usuario.Id };
                context.ClienteUsuarios.Add(clienteUsuario);
                await context.SaveChangesAsync();

                return true;
            }
            catch
            {
                throw new InvalidOperationException("Error");
            }
        }

        private string HashContrasenya(string contrasenya)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(contrasenya));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
}
