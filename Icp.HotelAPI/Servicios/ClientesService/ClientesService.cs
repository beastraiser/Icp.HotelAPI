using AutoMapper;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context;
using Icp.HotelAPI.Controllers.ClientesController.DTO;
using Icp.HotelAPI.Controllers.UsuariosController.DTO;
using Icp.HotelAPI.Servicios.ClientesService.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Icp.HotelAPI.Servicios.ClientesService
{
    public class ClientesService : IClienteService
    {
        private readonly FCT_ABR_11Context context;
        private readonly IMapper mapper;

        public ClientesService(
            FCT_ABR_11Context context,
            IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<ActionResult<ClienteDTO>> ObtenerClientePorDni(ClienteDniDTO clienteDniDTO)
        {
            var entidad = await context.Clientes.FirstOrDefaultAsync(x => x.Dni == clienteDniDTO.Dni);
            if (entidad == null)
            {
                throw new InvalidOperationException("Cliente no encontrado");
            }
            return mapper.Map<ClienteDTO>(entidad);
        }

        public async Task<bool> BorrarCliente(int id)
        {
            var cliente = await context.Clientes.FirstOrDefaultAsync(c => c.Id == id);

            if (cliente == null)
            {
                return false;
            }

            var clienteUsuario = await context.ClienteUsuarios.FirstOrDefaultAsync(cu => cu.IdCliente == id);

            if (clienteUsuario != null)
            {
                var usuario = await context.Usuarios.FirstOrDefaultAsync(u => u.Id == clienteUsuario.IdUsuario);

                if (usuario != null)
                {
                    context.ClienteUsuarios.Remove(clienteUsuario);
                    context.Usuarios.Remove(usuario);
                }
            }

            context.Clientes.Remove(cliente);

            await context.SaveChangesAsync();
            return true;
        }

    }
}
