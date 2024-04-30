using System;
using System.Collections.Generic;

namespace Icp.HotelAPI
{
    public partial class Servicio
    {
        public Servicio()
        {
            ReservaHabitacionServicios = new HashSet<ReservaHabitacionServicio>();
        }

        public int Id { get; set; }
        public string Tipo { get; set; }
        public string Descripcion { get; set; }
        public decimal Coste { get; set; }

        public virtual ICollection<ReservaHabitacionServicio> ReservaHabitacionServicios { get; set; }
    }
}
