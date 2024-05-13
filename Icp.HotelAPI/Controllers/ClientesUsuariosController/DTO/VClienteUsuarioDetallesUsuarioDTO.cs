namespace Icp.HotelAPI.Controllers.ClientesUsuariosController.DTO
{
    public class VClienteUsuarioDetallesUsuarioDTO
    {
        public int IdUsuario { get; set; }
        public string Email { get; set; }
        public string Contrasenya { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int IdPerfil { get; set; }
        public string Dni { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Telefono { get; set; }
    }
}
