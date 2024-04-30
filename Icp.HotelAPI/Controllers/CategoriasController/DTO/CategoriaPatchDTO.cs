using Icp.HotelAPI.Controllers.CategoriasController.Filtros;
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
    }
}
