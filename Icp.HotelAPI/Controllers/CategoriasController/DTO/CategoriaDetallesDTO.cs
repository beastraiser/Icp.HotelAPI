using Icp.HotelAPI.Controllers.TipoCamasController.DTO;

namespace Icp.HotelAPI.Controllers.CategoriasController.DTO
{
    public class CategoriaDetallesDTO: CategoriaDTO
    {
        public List<TipoCamaDetallesDTO> TipoCamas { get; set; }
    }
}
