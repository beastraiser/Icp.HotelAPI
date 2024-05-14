namespace Icp.HotelAPI.BBDD.FCT_ABR_11Context.Entidades
{
    public partial class ClienteUsuario
    {
        public int IdCliente { get; set; }
        public int IdUsuario { get; set; }

        public virtual Cliente IdClienteNavigation { get; set; }
        public virtual Usuario IdUsuarioNavigation { get; set; }
    }
}
