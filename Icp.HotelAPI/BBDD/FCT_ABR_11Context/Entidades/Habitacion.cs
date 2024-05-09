using Icp.HotelAPI.Controllers.Interfaces;
using System;
using System.Collections.Generic;

namespace Icp.HotelAPI.BBDD.FCT_ABR_11Context.Entidades
{
    public partial class Habitacion: IId
    {
        public Habitacion()
        {
            ReservaHabitacionServicios = new HashSet<ReservaHabitacionServicio>();
        }

        public int Id { get; set; }
        public int IdCategoria { get; set; }
        public bool Disponibilidad { get; set; }

        public virtual Categoria IdCategoriaNavigation { get; set; }
        public virtual ICollection<ReservaHabitacionServicio> ReservaHabitacionServicios { get; set; }
    }
}
