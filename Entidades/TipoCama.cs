using System;
using System.Collections.Generic;

namespace Icp.HotelAPI
{
    public partial class TipoCama
    {
        public byte NumeroHabitacion { get; set; }
        public string TipoCama1 { get; set; }

        public virtual Habitacion NumeroHabitacionNavigation { get; set; }
    }
}
