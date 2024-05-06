using System;
using System.Collections.Generic;

namespace Icp.HotelAPI.BBDD.FCT_ABR_11Context.Entidades
{
    public partial class TipoCama
    {
        public int IdCategoria { get; set; }
        public string Tipo { get; set; }

        public virtual Categoria IdCategoriaNavigation { get; set; }
    }
}
