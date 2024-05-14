using AutoMapper;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context.Entidades;
using Icp.HotelAPI.Controllers.CategoriasController.DTO;
using Icp.HotelAPI.Controllers.ClientesController.DTO;
using Icp.HotelAPI.Controllers.ClientesUsuariosController.DTO;
using Icp.HotelAPI.Controllers.HabitacionesController.DTO;
using Icp.HotelAPI.Controllers.PerfilesController.DTO;
using Icp.HotelAPI.Controllers.ReservasController.DTO;
using Icp.HotelAPI.Controllers.ReservasHabitacionesServiciosController.DTO;
using Icp.HotelAPI.Controllers.ServiciosController.DTO;
using Icp.HotelAPI.Controllers.TipoCamasController.DTO;
using Icp.HotelAPI.Controllers.UsuariosController.DTO;

namespace Icp.HotelAPI.ServiciosCompartidos.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Habitacion, HabitacionDTO>().ReverseMap();
            CreateMap<HabitacionPatchDTO, Habitacion>().ReverseMap();

            CreateMap<Servicio, ServicioDTO>().ReverseMap();
            CreateMap<ServicioCreacionDTO, Servicio>().ReverseMap();

            CreateMap<Cliente, ClienteDTO>().ReverseMap();
            CreateMap<ClienteCreacionDTO, Cliente>().ReverseMap();

            CreateMap<Usuario, UsuarioDTO>().ReverseMap();
            CreateMap<UsuarioCreacionDTO, Usuario>().ReverseMap();

            CreateMap<ClienteUsuarioDTO, Cliente>(); 
            CreateMap<ClienteUsuarioDTO, Usuario>() 
                .ForMember(dest => dest.IdPerfil, opt => opt.MapFrom(src => 4)); // IDPerfil tiene un valor predeterminado de 4

            CreateMap<VClienteUsuarioDTO, VClienteUsuario>().ReverseMap();
            CreateMap<VClienteUsuarioDetallesClienteDTO, VClienteUsuario>().ReverseMap();
            CreateMap<VClienteUsuarioDetallesUsuarioDTO, VClienteUsuario>().ReverseMap();

            CreateMap<Perfil, PerfilDTO>().ReverseMap();
            CreateMap<PerfilCreacionDTO, Perfil>().ReverseMap();

            CreateMap<Categoria, CategoriaDTO>().ReverseMap();
            CreateMap<CategoriaCreacionDTO, Categoria>()
                .ForMember(x => x.TipoCamas, options => options.Ignore())
                // Lógica para que ignore el campo Foto
                .ForMember(x => x.Foto, options => options.Ignore())
                .ReverseMap();
            CreateMap<CategoriaPatchDTO, Categoria>().ReverseMap();
            CreateMap<Categoria, CategoriaDetallesDTO>()
                .ForMember(x => x.TipoCamas, options => options.MapFrom(MapCategoriaTipoCamas));

            CreateMap<TipoCama, TipoCamaDTO>().ReverseMap();

            CreateMap<Reserva, ReservaDetallesDTO>()
                .ForMember(x => x.ReservaHabitacionServicios, options => options.MapFrom(MapReservaHabitacionServicio));
            CreateMap<Reserva, ReservaDetallesServicioDTO>()
                .ForMember(x => x.ReservaHabitacionServicios, options => options.MapFrom(MapReservaHabitacionServicioDetalles));
            CreateMap<ReservaCreacionDetallesDTO, Reserva>()
                .ForMember(x => x.ReservaHabitacionServicios, options => options.Ignore());
            CreateMap<Reserva, ReservaCreacionDetallesDTO>()
                .ForMember(x => x.ReservaHabitacionServicios, options => options.MapFrom(MapReservaHabitacionServicioModificacion));
            CreateMap<Reserva, ReservaDetallesCosteDTO>()
                .ForMember(x => x.ReservaHabitacionServicios, options => options.MapFrom(MapReservaHabitacionServicioCoste));

            CreateMap<ReservaHabitacionServicio, ReservaHabitacionServicioDTO>().ReverseMap();
        }

        private List<TipoCamaDetallesDTO> MapCategoriaTipoCamas(Categoria categoria, CategoriaDetallesDTO categoriaDetallesDTO)
        {
            var resultado = new List<TipoCamaDetallesDTO>();
            if (categoria.TipoCamas == null)
            {
                return resultado;
            }
            foreach (var tipoCama in categoria.TipoCamas)
            {
                resultado.Add(new TipoCamaDetallesDTO() { Tipo = tipoCama.Tipo });
            }
            return resultado;
        }

        private List<ReservaHabitacionServicioDetallesDTO> MapReservaHabitacionServicio(Reserva reserva, ReservaDetallesDTO reservaDetallesDTO)
        {
            var resultado = new List<ReservaHabitacionServicioDetallesDTO>();
            if (reserva.ReservaHabitacionServicios == null)
            {
                return resultado;
            }
            foreach (var habitacionServicio in reserva.ReservaHabitacionServicios)
            {
                resultado.Add(new ReservaHabitacionServicioDetallesDTO() { IdHabitacion = habitacionServicio.IdHabitacion, IdServicio = habitacionServicio.IdServicio });
            }
            return resultado;
        }

        private List<ReservaHabitacionServicoDetallesServicioDTO> MapReservaHabitacionServicioDetalles(Reserva reserva, ReservaDetallesServicioDTO reservaDetallesServicioDTO)
        {
            var resultado = new List<ReservaHabitacionServicoDetallesServicioDTO>();
            if (reserva.ReservaHabitacionServicios == null)
            {
                return resultado;
            }
            foreach (var habitacionServicio in reserva.ReservaHabitacionServicios)
            {
                resultado.Add(new ReservaHabitacionServicoDetallesServicioDTO() { IdServicio = habitacionServicio.IdServicio });
            }
            return resultado;
        }

        private List<ReservaHabitacionServicioDetallesDTO> MapReservaHabitacionServicioModificacion(Reserva reserva, ReservaCreacionDetallesDTO reservaCreacionDetallesDTO)
        {
            var resultado = new List<ReservaHabitacionServicioDetallesDTO>();
            if (reserva.ReservaHabitacionServicios == null)
            {
                return resultado;
            }
            foreach (var habitacionServicio in reserva.ReservaHabitacionServicios)
            {
                resultado.Add(new ReservaHabitacionServicioDetallesDTO() { IdHabitacion = habitacionServicio.IdHabitacion, IdServicio = habitacionServicio.IdServicio });
            }
            return resultado;
        }

        private List<ReservaHabitacionServicioDetallesDTO> MapReservaHabitacionServicioCoste(Reserva reserva, ReservaDetallesCosteDTO reservaDetallesCosteDTO)
        {
            var resultado = new List<ReservaHabitacionServicioDetallesDTO>();
            if (reserva.ReservaHabitacionServicios == null)
            {
                return resultado;
            }
            foreach (var habitacionServicio in reserva.ReservaHabitacionServicios)
            {
                resultado.Add(new ReservaHabitacionServicioDetallesDTO() { IdHabitacion = habitacionServicio.IdHabitacion, IdServicio = habitacionServicio.IdServicio });
            }
            return resultado;
        }
    }
}
