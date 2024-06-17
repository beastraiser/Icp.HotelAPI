using System.ComponentModel.DataAnnotations;

namespace Icp.HotelAPI.Controllers.HabitacionesController.DTO
{
    public class HabitacionPatchDTO
    {
        [Required]
        public int IdCategoria { get; set; }
    }
}
