using System;
using System.Collections.Generic;

namespace Icp.HotelAPI.BBDD.FCT_ABR_11Context.Entidades
{
    public partial class TipoCama
    {
        public byte NumeroHabitacion { get; set; }
        public string TipoCama1 { get; set; }

        public virtual Habitacion NumeroHabitacionNavigation { get; set; }
    }
}
