using System.ComponentModel.DataAnnotations;

namespace Icp.HotelAPI.Controllers.ClientesUsuariosController.DTO
{
    public class ClienteUsuarioDTO
    {
        [Required]
        public string DNI { get; set; }

        [Required]
        public string Telefono { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Apellidos { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Contrasenya { get; set; }
    }
}
