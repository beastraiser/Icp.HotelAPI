using AutoMapper;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context.Entidades;
using Icp.HotelAPI.Controllers.CategoriasController.DTO;
using Icp.HotelAPI.Controllers.HabitacionesController.DTO;
using Icp.HotelAPI.Controllers.PerfilesController.DTO;
using Icp.HotelAPI.Controllers.TipoCamasController.DTO;

namespace Icp.HotelAPI.ServiciosCompartidos.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Habitacion, HabitacionDTO>().ReverseMap();
            CreateMap<HabitacionPatchDTO, Habitacion>().ReverseMap();

            CreateMap<Perfil, PerfilDTO>().ReverseMap();
            CreateMap<PerfilCreacionDTO, Perfil>().ReverseMap();

            CreateMap<Categoria, CategoriaDTO>().ReverseMap();
            CreateMap<CategoriaCreacionDTO, Categoria>().ReverseMap()
                // Lógica para que ignore el campo Foto
                .ForMember(x => x.Foto, options => options.Ignore());
            CreateMap<CategoriaPatchDTO, Categoria>().ReverseMap();
            CreateMap<Categoria, CategoriaDetallesDTO>()
                .ForMember(x => x.TipoCamas, options => options.MapFrom(MapCategoriaTipoCamas));

            CreateMap<TipoCama, TipoCamaDTO>().ReverseMap();
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


    }
}
