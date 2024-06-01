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
    }
}
