using System.ComponentModel.DataAnnotations;

namespace Icp.HotelAPI.Controllers.UsuariosController.DTO
{
    public class EditarPermisoDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
