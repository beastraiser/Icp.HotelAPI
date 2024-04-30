using System;
using System.Collections.Generic;

namespace Icp.HotelAPI
{
    public partial class VGrant
    {
        public string Usuario { get; set; }
        public string TipoPermiso { get; set; }
        public string NombrePermiso { get; set; }
        public string NombreObjeto { get; set; }
        public string Tipo { get; set; }
        public string Columna { get; set; }
        public string Grant { get; set; }
        public string Esquema { get; set; }
    }
}
