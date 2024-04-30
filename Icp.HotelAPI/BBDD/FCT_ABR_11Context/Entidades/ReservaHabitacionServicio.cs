using System;
using System.Collections.Generic;

namespace Icp.HotelAPI.BBDD.FCT_ABR_11Context.Entidades
{
    public partial class ReservaHabitacionServicio
    {
        public int IdReserva { get; set; }
        public byte NumeroHabitacion { get; set; }
        public int IdServicio { get; set; }

        public virtual Reserva IdReservaNavigation { get; set; }
        public virtual Servicio IdServicioNavigation { get; set; }
        public virtual Habitacion NumeroHabitacionNavigation { get; set; }
    }
}
