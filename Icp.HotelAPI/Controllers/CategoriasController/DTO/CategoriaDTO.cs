namespace Icp.HotelAPI.Controllers.CategoriasController.DTO
{
    public class CategoriaDTO
    {
        public int Id { get; set; }
        public string Tipo { get; set; }
        public byte NumeroCamas { get; set; }
        public byte MaximoPersonas { get; set; }
        public decimal CosteNoche { get; set; }
        public string Foto { get; set; }
    }
}
