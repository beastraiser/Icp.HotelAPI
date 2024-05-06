using System;
using System.Collections.Generic;

namespace Icp.HotelAPI
{
    public partial class Habitacion
    {
        public Habitacion()
        {
            ReservaHabitacionServicios = new HashSet<ReservaHabitacionServicio>();
        }

        public byte Numero { get; set; }
        public int IdCategoria { get; set; }
        public bool Disponibilidad { get; set; }

        public virtual Categorium IdCategoriaNavigation { get; set; }
        public virtual ICollection<ReservaHabitacionServicio> ReservaHabitacionServicios { get; set; }
    }
}
