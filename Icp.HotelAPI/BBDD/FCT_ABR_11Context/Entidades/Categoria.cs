using System;
using System.Collections.Generic;

namespace Icp.HotelAPI.BBDD.FCT_ABR_11Context.Entidades
{
    public partial class Categoria
    {
        public Categoria()
        {
            Habitacions = new HashSet<Habitacion>();
        }

        public int Id { get; set; }
        public string Tipo { get; set; }
        public byte NumeroCamas { get; set; }
        public byte MaximoPersonas { get; set; }
        public decimal CosteNoche { get; set; }
        public string Foto { get; set; }

        public virtual ICollection<Habitacion> Habitacions { get; set; }
    }
}
