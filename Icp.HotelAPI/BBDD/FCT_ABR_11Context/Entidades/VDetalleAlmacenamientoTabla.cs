using System;
using System.Collections.Generic;

namespace Icp.HotelAPI.BBDD.FCT_ABR_11Context.Entidades
{
    public partial class VDetalleAlmacenamientoTabla
    {
        public string Tabla { get; set; }
        public string Esquema { get; set; }
        public long? Filas { get; set; }
        public int? TotalMb { get; set; }
        public int? LibreMb { get; set; }
        public string PorcentajeLibre { get; set; }
        public string Columna { get; set; }
        public int? IdIndiceColumna { get; set; }
        public string NombreIndice { get; set; }
        public string TipoIndice { get; set; }
        public bool? Pk { get; set; }
        public byte? NumColumna { get; set; }
        public bool? ClaveUnica { get; set; }
        public bool? Incluida { get; set; }
        public string CampoIdentity { get; set; }
        public string DatosGrupoFicheros { get; set; }
        public string DatosFichero { get; set; }
        public string DatosRutaFichero { get; set; }
        public string IndicesGrupoFichero { get; set; }
        public string IndicesFichero { get; set; }
        public string IndicesRutaFichero { get; set; }
        public string LobGrupoFicheros { get; set; }
        public string LobFichero { get; set; }
        public string LobRutaFichero { get; set; }
    }
}
