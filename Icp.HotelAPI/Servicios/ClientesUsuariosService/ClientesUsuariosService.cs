using AutoMapper;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context.Entidades;
using Icp.HotelAPI.Controllers.ClientesUsuariosController.DTO;
using Icp.HotelAPI.Servicios.ClientesUsuariosService.Interfaces;
using Icp.HotelAPI.ServiciosCompartidos.LoginService.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Icp.HotelAPI.Servicios.ClientesUsuariosService
{
    public class ClientesUsuariosService : IClienteUsuarioService
    {
        private readonly FCT_ABR_11Context context;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;
        private readonly ILoginService loginService;

        public ClientesUsuariosService(FCT_ABR_11Context context,
            IMapper mapper,
            IConfiguration configuration,
            ILoginService loginService)
        {
            this.context = context;
            this.mapper = mapper;
            this.configuration = configuration;
            this.loginService = loginService;
        }

        public async Task<VClienteUsuarioDetallesUsuarioDTO> ObtenerClienteUsuarioPorIdCliente(int id)
        {
            var entidad = await context.VClienteUsuarios.FirstOrDefaultAsync(x => x.IdCliente == id);
            if (entidad == null)
            {
                throw new InvalidOperationException("El cliente no existe");
            }
            return mapper.Map<VClienteUsuarioDetallesUsuarioDTO>(entidad);
        }

        public async Task<VClienteUsuarioDetallesClienteDTO> ObtenerClienteUsuarioPorIdUsuario(int id)
        {
            var entidad = await context.VClienteUsuarios.FirstOrDefaultAsync(x => x.IdUsuario == id);
            if (entidad == null)
            {
                throw new InvalidOperationException("El usuario no existe");
            }
            var dto = mapper.Map<VClienteUsuarioDetallesClienteDTO>(entidad);
            return dto;
        }

        public async Task<bool> Registrar(ClienteUsuarioDTO clienteUsuarioDTO)
        {
            // Verificaciones
            var existeCliente = await context.Clientes.FirstOrDefaultAsync(x => x.Dni == clienteUsuarioDTO.DNI);
            Cliente cliente;

            if (existeCliente != null)
            {
                // Modifica el cliente con los últimos datos recibidos
                cliente = existeCliente;
                mapper.Map(clienteUsuarioDTO, cliente);
                context.Entry(cliente).State = EntityState.Modified;
            }
            else
            {
                // Crea el cliente
                cliente = mapper.Map<Cliente>(clienteUsuarioDTO);
                context.Clientes.Add(cliente);
            }
            

            // Verificar si el cliente ya tiene una cuenta asociada antes de crear el usuario
            var existeClienteUsuario = await context.ClienteUsuarios.FirstOrDefaultAsync(c => c.IdCliente == cliente.Id);
            if (existeClienteUsuario != null)
            {
                throw new InvalidOperationException($"Este cliente ya tiene una cuenta de usuario asociada");
            }

            // Verificar si el usuario ya existe
            var existeUsuario = await context.Usuarios.FirstOrDefaultAsync(x => x.Email == clienteUsuarioDTO.Email);
            if (existeUsuario != null)
            {
                throw new InvalidOperationException($"El email {clienteUsuarioDTO.Email} ya existe");
            }
            await context.SaveChangesAsync();

            // Crear el usuario
            var usuario = mapper.Map<Usuario>(clienteUsuarioDTO);
            usuario.Contrasenya = loginService.HashContrasenya(usuario.Contrasenya);
            context.Usuarios.Add(usuario);
            await context.SaveChangesAsync();

            // Asociar el cliente y el usuario
            var clienteUsuario = new ClienteUsuario { IdCliente = cliente.Id, IdUsuario = usuario.Id };
            context.ClienteUsuarios.Add(clienteUsuario);
            await context.SaveChangesAsync();

            return true;
        }
    }
}
