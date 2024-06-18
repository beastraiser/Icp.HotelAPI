using System.ComponentModel.DataAnnotations;

namespace Icp.HotelAPI.Controllers.UsuariosController.DTO
{
    public class UsuarioAltaDTO
    {
        [Required]
        public string Dni { get; set; }
    }
}
