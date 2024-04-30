using System;
using System.Collections.Generic;

namespace Icp.HotelAPI
{
    public partial class Habitacion
    {
        public Habitacion()
        {
            ReservaHabitacionServicios = new HashSet<ReservaHabitacionServicio>();
            TipoCamas = new HashSet<TipoCama>();
        }

        public byte Numero { get; set; }
        public int Categoria { get; set; }
        public bool Disponibilidad { get; set; }

        public virtual Categorium CategoriaNavigation { get; set; }
        public virtual ICollection<ReservaHabitacionServicio> ReservaHabitacionServicios { get; set; }
        public virtual ICollection<TipoCama> TipoCamas { get; set; }
    }
}
