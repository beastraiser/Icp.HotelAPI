using System;
using System.Collections.Generic;

namespace Icp.HotelAPI.BBDD.FCT_ABR_11Context.Entidades
{
    public partial class VDetalleForeignKey
    {
        public string NomFk { get; set; }
        public string Tabla { get; set; }
        public string Esquema { get; set; }
        public string Columna { get; set; }
        public string TablaReferencia { get; set; }
        public string EsquemaReferencia { get; set; }
        public string ColumnaReferencia { get; set; }
        public byte? TipoBorradoEnCascada { get; set; }
        public byte? TipoActualizacionEnCascada { get; set; }
    }
}
