using Icp.HotelAPI.ServiciosCompartidos.PaginacionDTO;

namespace Icp.HotelAPI.Controllers.HabitacionesController.DTO
{
    public class FiltroHabitacionDTO
    {
        public int Pagina { get; set; } = 1;
        public int CantidadRegistrosPorPagina { get; set; } = 10;
        public PaginacionDTO Paginacion
        {
            get
            {
                return new PaginacionDTO() { Pagina = Pagina, CantidadRegistrosPorPagina = CantidadRegistrosPorPagina };
            }
        }

        public int IdCategoria { get; set; }
    }
}
