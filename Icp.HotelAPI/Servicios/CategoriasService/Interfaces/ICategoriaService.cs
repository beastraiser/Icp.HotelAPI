using Icp.HotelAPI.Controllers.CategoriasController.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Icp.HotelAPI.Servicios.CategoriasService.Interfaces
{
    public interface ICategoriaService
    {
        Task<List<CategoriaDetallesDTO>> ObtenerCategorias();
        Task<CategoriaDetallesDTO> ObtenerCategoriaId(int id);
        Task<ActionResult> NuevaCategoria([FromForm] CategoriaCreacionDTO categoriaCreacionDTO);
        Task<bool> CambiarDatosCategoria(int id, [FromForm] CategoriaCreacionDTO categoriaCreacionDTO);
        Task<bool> BorrarCategoria(int id);
    }
}
