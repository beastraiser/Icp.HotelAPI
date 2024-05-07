using System.ComponentModel.DataAnnotations;

namespace Icp.HotelAPI.Controllers.HabitacionesController.DTO
{
    public class HabitacionDTO: HabitacionPatchDTO
    {
        [Required]
        public byte Id { get; set; }
    }
}
