using System;
using System.Collections.Generic;

namespace Icp.HotelAPI.BBDD.FCT_ABR_11Context.Entidades
{
    public partial class Reserva
    {
        public Reserva()
        {
            ReservaHabitacionServicios = new HashSet<ReservaHabitacionServicio>();
        }

        public int Id { get; set; }
        public int IdCliente { get; set; }
        public int IdUsuario { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public decimal CosteTotal { get; set; }

        public virtual Cliente IdClienteNavigation { get; set; }
        public virtual Usuario IdUsuarioNavigation { get; set; }
        public virtual ICollection<ReservaHabitacionServicio> ReservaHabitacionServicios { get; set; }
    }
}
