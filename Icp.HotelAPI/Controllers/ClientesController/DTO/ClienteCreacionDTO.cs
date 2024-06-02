namespace Icp.HotelAPI.Controllers.ClientesController.DTO
{
    public class ClienteCreacionDTO : ClienteDniDTO
    {
        public string Telefono { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
    }
}
