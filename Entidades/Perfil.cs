using System;
using System.Collections.Generic;

namespace Icp.HotelAPI
{
    public partial class Perfil
    {
        public Perfil()
        {
            Permisos = new HashSet<Permiso>();
            Usuarios = new HashSet<Usuario>();
        }

        public int Id { get; set; }
        public string Tipo { get; set; }

        public virtual ICollection<Permiso> Permisos { get; set; }
        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
