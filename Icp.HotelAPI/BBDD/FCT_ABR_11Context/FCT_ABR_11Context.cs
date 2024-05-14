using Microsoft.EntityFrameworkCore;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context.Entidades;

namespace Icp.HotelAPI.BBDD.FCT_ABR_11Context
{
    public partial class FCT_ABR_11Context : DbContext
    {
        public FCT_ABR_11Context()
        {
        }

        public FCT_ABR_11Context(DbContextOptions<FCT_ABR_11Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Categoria> Categorias { get; set; }
        public virtual DbSet<Cliente> Clientes { get; set; }
        public virtual DbSet<ClienteUsuario> ClienteUsuarios { get; set; }
        public virtual DbSet<Habitacion> Habitaciones { get; set; }
        public virtual DbSet<Perfil> Perfiles { get; set; }
        public virtual DbSet<Reserva> Reservas { get; set; }
        public virtual DbSet<ReservaHabitacionServicio> ReservaHabitacionServicios { get; set; }
        public virtual DbSet<Servicio> Servicios { get; set; }
        public virtual DbSet<TipoCama> TipoCamas { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<VClienteUsuario> VClienteUsuarios { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.ToTable("CATEGORIA");

                entity.HasIndex(e => e.Tipo, "UQ__CATEGORI__B6FCAAA246AE1AF3")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CosteNoche)
                    .HasColumnType("decimal(6, 2)")
                    .HasColumnName("COSTE_NOCHE");

                entity.Property(e => e.Foto)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("FOTO");

                entity.Property(e => e.MaximoPersonas).HasColumnName("MAXIMO_PERSONAS");

                entity.Property(e => e.NumeroCamas).HasColumnName("NUMERO_CAMAS");

                entity.Property(e => e.Tipo)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("TIPO");
            });

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.ToTable("CLIENTE");

                entity.HasIndex(e => e.Dni, "UQ__CLIENTE__C035B8DD8C6FF5C0")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Apellidos)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("APELLIDOS");

                entity.Property(e => e.Dni)
                    .IsRequired()
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("DNI")
                    .IsFixedLength();

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE");

                entity.Property(e => e.Telefono)
                    .IsRequired()
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("TELEFONO")
                    .IsFixedLength();
            });

            modelBuilder.Entity<ClienteUsuario>(entity =>
            {
                entity.HasKey(e => new { e.IdCliente, e.IdUsuario })
                    .HasName("PK__CLIENTE___3AB27789EE347688");

                entity.ToTable("CLIENTE_USUARIO");

                entity.HasIndex(e => e.IdCliente, "UQ__CLIENTE___23A341310E3A2826")
                    .IsUnique();

                entity.HasIndex(e => e.IdUsuario, "UQ__CLIENTE___91136B918758943F")
                    .IsUnique();

                entity.Property(e => e.IdCliente).HasColumnName("ID_CLIENTE");

                entity.Property(e => e.IdUsuario).HasColumnName("ID_USUARIO");

                entity.HasOne(d => d.IdClienteNavigation)
                    .WithOne(p => p.ClienteUsuario)
                    .HasForeignKey<ClienteUsuario>(d => d.IdCliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CLIENTE_USUARIO_CLIENTE");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithOne(p => p.ClienteUsuario)
                    .HasForeignKey<ClienteUsuario>(d => d.IdUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CLIENTE_USUARIO_USUARIO");
            });

            modelBuilder.Entity<Habitacion>(entity =>
            {
                entity.ToTable("HABITACION");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.Disponibilidad).HasColumnName("DISPONIBILIDAD");

                entity.Property(e => e.IdCategoria).HasColumnName("ID_CATEGORIA");

                entity.HasOne(d => d.IdCategoriaNavigation)
                    .WithMany(p => p.Habitaciones)
                    .HasForeignKey(d => d.IdCategoria)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ID_CATEGORIA_HABITACION");
            });

            modelBuilder.Entity<Perfil>(entity =>
            {
                entity.ToTable("PERFIL");

                entity.HasIndex(e => e.Tipo, "UQ__PERFIL__B6FCAAA293D31557")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Tipo)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("TIPO");
            });

            modelBuilder.Entity<Reserva>(entity =>
            {
                entity.ToTable("RESERVA");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cancelada)
                    .HasColumnName("CANCELADA")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.CosteTotal)
                    .HasColumnType("decimal(6, 2)")
                    .HasColumnName("COSTE_TOTAL");

                entity.Property(e => e.FechaFin)
                    .HasColumnType("date")
                    .HasColumnName("FECHA_FIN");

                entity.Property(e => e.FechaInicio)
                    .HasColumnType("date")
                    .HasColumnName("FECHA_INICIO");

                entity.Property(e => e.IdCliente).HasColumnName("ID_CLIENTE");

                entity.Property(e => e.IdUsuario).HasColumnName("ID_USUARIO");

                entity.HasOne(d => d.IdClienteNavigation)
                    .WithMany(p => p.Reservas)
                    .HasForeignKey(d => d.IdCliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CLIENTE_RESERVA");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Reservas)
                    .HasForeignKey(d => d.IdUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_USUARIO_RESERVA");
            });

            modelBuilder.Entity<ReservaHabitacionServicio>(entity =>
            {
                entity.HasKey(e => new { e.IdReserva, e.IdHabitacion, e.IdServicio })
                    .HasName("PK__RESERVA___EFA0D396614490FA");

                entity.ToTable("RESERVA_HABITACION_SERVICIO");

                entity.Property(e => e.IdReserva).HasColumnName("ID_RESERVA");

                entity.Property(e => e.IdHabitacion).HasColumnName("ID_HABITACION");

                entity.Property(e => e.IdServicio).HasColumnName("ID_SERVICIO");

                entity.HasOne(d => d.IdHabitacionNavigation)
                    .WithMany(p => p.ReservaHabitacionServicios)
                    .HasForeignKey(d => d.IdHabitacion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HABITACION_RESERVA_HABITACION_SERVICIO");

                entity.HasOne(d => d.IdReservaNavigation)
                    .WithMany(p => p.ReservaHabitacionServicios)
                    .HasForeignKey(d => d.IdReserva)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RESERVA_RESERVA_HABITACION_SERVICIO");

                entity.HasOne(d => d.IdServicioNavigation)
                    .WithMany(p => p.ReservaHabitacionServicios)
                    .HasForeignKey(d => d.IdServicio)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SERVICIO_RESERVA_HABITACION_SERVICIO");
            });

            modelBuilder.Entity<Servicio>(entity =>
            {
                entity.ToTable("SERVICIO");

                entity.HasIndex(e => e.Nombre, "UQ__SERVICIO__B21D0AB925431B71")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Coste)
                    .HasColumnType("decimal(6, 2)")
                    .HasColumnName("COSTE");

                entity.Property(e => e.Descripcion)
                    .HasColumnType("text")
                    .HasColumnName("DESCRIPCION");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE");

                entity.Property(e => e.Tipo)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("TIPO");
            });

            modelBuilder.Entity<TipoCama>(entity =>
            {
                entity.HasKey(e => new { e.IdCategoria, e.Tipo })
                    .HasName("PK__TIPO_CAM__60BAD50FFB310741");

                entity.ToTable("TIPO_CAMA");

                entity.Property(e => e.IdCategoria).HasColumnName("ID_CATEGORIA");

                entity.Property(e => e.Tipo)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("TIPO");

                entity.HasOne(d => d.IdCategoriaNavigation)
                    .WithMany(p => p.TipoCamas)
                    .HasForeignKey(d => d.IdCategoria)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ID_CATEGORIA_TIPO_CAMA");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("USUARIO");

                entity.HasIndex(e => e.Email, "UQ__USUARIO__161CF7247438046A")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Contrasenya)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("CONTRASENYA");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("EMAIL");

                entity.Property(e => e.FechaRegistro)
                    .HasColumnType("date")
                    .HasColumnName("FECHA_REGISTRO");

                entity.Property(e => e.IdPerfil)
                    .HasColumnName("ID_PERFIL")
                    .HasDefaultValueSql("((4))");

                entity.HasOne(d => d.IdPerfilNavigation)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.IdPerfil)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PERFIL_USUARIO");
            });

            modelBuilder.Entity<VClienteUsuario>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_CLIENTE_USUARIO");

                entity.Property(e => e.Apellidos)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("APELLIDOS");

                entity.Property(e => e.Contrasenya)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("CONTRASENYA");

                entity.Property(e => e.Dni)
                    .IsRequired()
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("DNI")
                    .IsFixedLength();

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("EMAIL");

                entity.Property(e => e.FechaRegistro)
                    .HasColumnType("date")
                    .HasColumnName("FECHA_REGISTRO");

                entity.Property(e => e.IdCliente).HasColumnName("ID_CLIENTE");

                entity.Property(e => e.IdPerfil).HasColumnName("ID_PERFIL");

                entity.Property(e => e.IdUsuario).HasColumnName("ID_USUARIO");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE");

                entity.Property(e => e.Telefono)
                    .IsRequired()
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("TELEFONO")
                    .IsFixedLength();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
