using Icp.HotelAPI.Controllers.Interfaces;
using System;
using System.Collections.Generic;

namespace Icp.HotelAPI.BBDD.FCT_ABR_11Context.Entidades
{
    public partial class Servicio: IId
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
