using Icp.HotelAPI.Controllers.Interfaces;

namespace Icp.HotelAPI.BBDD.FCT_ABR_11Context.Entidades
{
    public partial class Habitacion : IId
    {
        public Habitacion()
        {
            ReservaHabitacionServicios = new HashSet<ReservaHabitacionServicio>();
        }

        public int Id { get; set; }
        public int IdCategoria { get; set; }

        public virtual Categoria IdCategoriaNavigation { get; set; }
        public virtual ICollection<ReservaHabitacionServicio> ReservaHabitacionServicios { get; set; }
    }
}
