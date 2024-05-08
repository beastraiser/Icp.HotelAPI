using Icp.HotelAPI.Controllers.CategoriasController.Filtros;
using Icp.HotelAPI.Controllers.Filtros;
using Icp.HotelAPI.Controllers.TipoCamasController.DTO;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Icp.HotelAPI.Controllers.CategoriasController.DTO
{
    public class CategoriaPatchDTO
    {
        [Required]
        public string Tipo { get; set; }

        public byte NumeroCamas { get; set; }
        public byte MaximoPersonas { get; set; }
        public decimal CosteNoche { get; set; }

        [ModelBinder(BinderType = typeof(TypeBinder<List<TipoCamaDetallesDTO>>))]
        public List<TipoCamaDetallesDTO> TipoCamas { get; set; }
    }
}
