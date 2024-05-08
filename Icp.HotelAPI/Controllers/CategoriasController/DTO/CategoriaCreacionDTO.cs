using Icp.HotelAPI.BBDD.FCT_ABR_11Context.Entidades;
using Icp.HotelAPI.Controllers.CategoriasController.Filtros;
using Icp.HotelAPI.Controllers.Filtros;
using Icp.HotelAPI.Controllers.TipoCamasController.DTO;
using Microsoft.AspNetCore.Mvc;
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
