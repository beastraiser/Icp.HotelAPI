using AutoMapper;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context;
using Icp.HotelAPI.Controllers.ServiciosController.DTO;
using Icp.HotelAPI.Servicios.ServiciosService.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Icp.HotelAPI.Servicios.ServiciosService
{
    public class ServiciosService : IServicioService
    {
        private readonly FCT_ABR_11Context context;
        private readonly IMapper mapper;

        public ServiciosService(
            FCT_ABR_11Context context,
            IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<List<ServicioDTO>> ObtenerServicios()
        {
            var entidades = await context.Servicios
                .Where(s => s.Tipo == "SERVICIO")
                .ToListAsync();

            var dtos = mapper.Map<List<ServicioDTO>>(entidades);
            return dtos;
        }

        public async Task<List<ServicioDTO>> ObtenerExtras()
        {
            var entidades = await context.Servicios
                .Where(s => s.Tipo == "EXTRA")
                .ToListAsync();

            var dtos = mapper.Map<List<ServicioDTO>>(entidades);
            return dtos;
        }
    }
}
