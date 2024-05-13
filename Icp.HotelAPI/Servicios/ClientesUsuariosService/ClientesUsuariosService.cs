using AutoMapper;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context.Entidades;
using Icp.HotelAPI.Controllers.ClientesUsuariosController.DTO;
using Microsoft.EntityFrameworkCore;

namespace Icp.HotelAPI.Servicios.ClientesUsuariosService
{
    public class ClientesUsuariosService
    {
        private readonly FCT_ABR_11Context context;
        private readonly IMapper mapper;

        public ClientesUsuariosService(FCT_ABR_11Context context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<bool> CrearClienteYUsuario(ClienteUsuarioDTO clienteUsuarioDTO)
        {
            try
            {
                // Crear el cliente
                var cliente = mapper.Map<Cliente>(clienteUsuarioDTO);
                context.Clientes.Add(cliente);
                await context.SaveChangesAsync();

                // Crear el usuario
                var usuario = mapper.Map<Usuario>(clienteUsuarioDTO);
                context.Usuarios.Add(usuario);
                await context.SaveChangesAsync();

                // Asociar el cliente y el usuario
                var clienteUsuario = new ClienteUsuario { IdCliente = cliente.Id, IdUsuario = usuario.Id };
                context.ClienteUsuarios.Add(clienteUsuario);
                await context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                // Manejar errores aquí
                return false;
            }
        }
    }
}
