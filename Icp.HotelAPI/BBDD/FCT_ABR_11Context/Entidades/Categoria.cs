using Icp.HotelAPI.Controllers.Interfaces;
using System;
using System.Collections.Generic;

namespace Icp.HotelAPI.BBDD.FCT_ABR_11Context.Entidades
{
    public partial class Categoria: IId
    {
        public Categoria()
        {
            Habitaciones = new HashSet<Habitacion>();
            TipoCamas = new HashSet<TipoCama>();
        }

        public int Id { get; set; }
        public string Tipo { get; set; }
        public byte NumeroCamas { get; set; }
        public byte MaximoPersonas { get; set; }
        public decimal CosteNoche { get; set; }
        public string Foto { get; set; }

        public virtual ICollection<Habitacion> Habitaciones { get; set; }
        public virtual ICollection<TipoCama> TipoCamas { get; set; }
    }
}
