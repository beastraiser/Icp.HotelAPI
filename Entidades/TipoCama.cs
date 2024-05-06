using System;
using System.Collections.Generic;

namespace Icp.HotelAPI
{
    public partial class TipoCama
    {
        public int IdCategoria { get; set; }
        public string Tipo { get; set; }

        public virtual Categorium IdCategoriaNavigation { get; set; }
    }
}
