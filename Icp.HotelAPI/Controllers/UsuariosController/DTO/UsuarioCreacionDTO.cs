namespace Icp.HotelAPI.Controllers.UsuariosController.DTO
{
    public class UsuarioCreacionDTO
    {
        public int IdPerfil { get; set; }
        public string Email { get; set; }
        public string Contrasenya { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
