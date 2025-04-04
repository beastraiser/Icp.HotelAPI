﻿namespace Icp.HotelAPI.BBDD.FCT_ABR_11Context.Entidades
{
    public partial class ReservaHabitacionServicio
    {
        public int IdReserva { get; set; }
        public int IdHabitacion { get; set; }
        public int? IdServicio { get; set; }

        public virtual Habitacion IdHabitacionNavigation { get; set; }
        public virtual Reserva IdReservaNavigation { get; set; }
        public virtual Servicio IdServicioNavigation { get; set; }
    }
}
