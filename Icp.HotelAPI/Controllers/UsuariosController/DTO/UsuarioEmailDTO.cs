using System.ComponentModel.DataAnnotations;

namespace Icp.HotelAPI.Controllers.UsuariosController.DTO
{
    public class UsuarioEmailDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
