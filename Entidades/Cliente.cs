﻿using System;
using System.Collections.Generic;

namespace Icp.HotelAPI
{
    public partial class Cliente
    {
        public Cliente()
        {
            Reservas = new HashSet<Reserva>();
        }

        public int Id { get; set; }
        public string Dni { get; set; }
        public string Telefono { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }

        public virtual ICollection<Reserva> Reservas { get; set; }
    }
}
