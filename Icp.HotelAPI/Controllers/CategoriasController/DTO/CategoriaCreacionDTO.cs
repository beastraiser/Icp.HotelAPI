using Icp.HotelAPI.Controllers.CategoriasController.Filtros;
using System.ComponentModel.DataAnnotations;

namespace Icp.HotelAPI.Controllers.CategoriasController.DTO
{
    public class CategoriaCreacionDTO: CategoriaPatchDTO
    {
        [PesoArchivoValidacion(PesoMaximoEnMegabytes: 4)]
        [TipoArchivoValidacion(grupoTipoArchivo: GrupoTipoArchivo.Imagen)]
        public IFormFile Foto { get; set; }
    }
}
