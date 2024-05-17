using System.ComponentModel.DataAnnotations;

namespace Icp.HotelAPI.Controllers.UsuariosController.DTO
{
    public class UsuarioCredencialesDTO : EditarPermisoDTO
    {
        [Required]
        public string Contrasenya { get; set; }
    }
}
