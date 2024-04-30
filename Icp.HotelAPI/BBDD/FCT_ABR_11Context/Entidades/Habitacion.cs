﻿using System;
using System.Collections.Generic;

namespace Icp.HotelAPI.BBDD.FCT_ABR_11Context.Entidades
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

        public virtual Categoria CategoriaNavigation { get; set; }
        public virtual ICollection<ReservaHabitacionServicio> ReservaHabitacionServicios { get; set; }
        public virtual ICollection<TipoCama> TipoCamas { get; set; }
    }
}
