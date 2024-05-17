namespace Icp.HotelAPI.Controllers.UsuariosController.DTO
{
    public class RespuestaAutenticacionDTO
    {
        public string Token { get; set; }
        public DateTime Expire {  get; set; }
    }
}
