using System;
using System.Collections.Generic;

namespace Icp.HotelAPI.BBDD.FCT_ABR_11Context.Entidades
{
    public partial class Permiso
    {
        public int IdPerfil { get; set; }
        public string Permiso1 { get; set; }

        public virtual Perfil IdPerfilNavigation { get; set; }
    }
}
