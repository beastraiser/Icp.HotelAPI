using AutoMapper;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context.Entidades;
using Icp.HotelAPI.Controllers.CategoriasController.DTO;
using Icp.HotelAPI.Controllers.HabitacionesController.DTO;
using Icp.HotelAPI.Controllers.PerfilesController.DTO;

namespace Icp.HotelAPI.ServiciosCompartidos.Helpers
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Habitacion, HabitacionDTO>().ReverseMap();
            CreateMap<HabitacionPatchDTO, Habitacion>().ReverseMap();

            CreateMap<Perfil, PerfilDTO>().ReverseMap();
            CreateMap<PerfilCreacionDTO,  Perfil>().ReverseMap();

            CreateMap<Categoria, CategoriaDTO>().ReverseMap();
            CreateMap<CategoriaCreacionDTO, Categoria>().ReverseMap()
                // Lógica para que ignore el campo Foto
                .ForMember(x => x.Foto, options => options.Ignore());
            CreateMap<CategoriaPatchDTO, Categoria>().ReverseMap();
        }
    }
}
