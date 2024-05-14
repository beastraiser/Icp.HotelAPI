using Icp.HotelAPI.Controllers.Interfaces;

namespace Icp.HotelAPI.BBDD.FCT_ABR_11Context.Entidades
{
    public partial class Usuario : IId
    {
        public Usuario()
        {
            Reservas = new HashSet<Reserva>();
        }

        public int Id { get; set; }
        public int IdPerfil { get; set; }
        public string Email { get; set; }
        public string Contrasenya { get; set; }
        public DateTime FechaRegistro { get; set; }

        public virtual Perfil IdPerfilNavigation { get; set; }
        public virtual ClienteUsuario ClienteUsuario { get; set; }
        public virtual ICollection<Reserva> Reservas { get; set; }
    }
}
