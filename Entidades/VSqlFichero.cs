using System;
using System.Collections.Generic;

namespace Icp.HotelAPI
{
    public partial class VSqlFichero
    {
        public string Bbdd { get; set; }
        public string Fichero { get; set; }
        public string RutaFichero { get; set; }
        public string GrupoFichero { get; set; }
        public int? TotalMb { get; set; }
        public int? LibreMb { get; set; }
        public int? PorcentajeLibre { get; set; }
    }
}
