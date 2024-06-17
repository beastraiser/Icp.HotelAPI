using System.ComponentModel.DataAnnotations;

namespace Icp.HotelAPI.Controllers.UsuariosController.DTO
{
    public class UsuarioCreacionDTO
    {
        [Required]
        public int IdPerfil { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Contrasenya { get; set; }

        public DateTime FechaRegistro { get; set; }

        public bool Baja { get; set; }
    }
}
