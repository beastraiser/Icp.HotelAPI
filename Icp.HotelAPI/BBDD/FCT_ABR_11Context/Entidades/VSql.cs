using System;
using System.Collections.Generic;

namespace Icp.HotelAPI.BBDD.FCT_ABR_11Context.Entidades
{
    public partial class VSql
    {
        public int Id { get; set; }
        public string Tabla { get; set; }
        public string Campo { get; set; }
        public string Descripcion { get; set; }
        public int? Posicion { get; set; }
        public string Tipo { get; set; }
        public int Dimension { get; set; }
        public int PermiteNulos { get; set; }
        public string Defecto { get; set; }
        public string Clave { get; set; }
        public int Indices { get; set; }
        public string Collate { get; set; }
        public string GrupoFicheros { get; set; }
        public int? Kbytes { get; set; }
        public int? Mbytes { get; set; }
        public int? Growth { get; set; }
        public int? NFiles { get; set; }
        public string Grupo { get; set; }
        public string Triggers { get; set; }
        public string Name { get; set; }
        public int? ObjectId { get; set; }
        public int? DataSpaceId { get; set; }
        public string Filegroup { get; set; }
    }
}
