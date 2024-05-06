using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Icp.HotelAPI
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

        public virtual DbSet<Categorium> Categoria { get; set; }
        public virtual DbSet<Cliente> Clientes { get; set; }
        public virtual DbSet<Habitacion> Habitacions { get; set; }
        public virtual DbSet<Perfil> Perfils { get; set; }
        public virtual DbSet<Reserva> Reservas { get; set; }
        public virtual DbSet<ReservaHabitacionServicio> ReservaHabitacionServicios { get; set; }
        public virtual DbSet<Servicio> Servicios { get; set; }
        public virtual DbSet<TipoCama> TipoCamas { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<VDetalleAlmacenamientoTabla> VDetalleAlmacenamientoTablas { get; set; }
        public virtual DbSet<VDetalleForeignKey> VDetalleForeignKeys { get; set; }
        public virtual DbSet<VGrant> VGrants { get; set; }
        public virtual DbSet<VSql> VSqls { get; set; }
        public virtual DbSet<VSqlFichero> VSqlFicheros { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=ps17;Database=FCT_ABR_11;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categorium>(entity =>
            {
                entity.ToTable("CATEGORIA");

                entity.HasIndex(e => e.Tipo, "UQ__CATEGORI__B6FCAAA21368DE35")
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

                entity.HasIndex(e => e.Dni, "UQ__CLIENTE__C035B8DDF99167F4")
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
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("TELEFONO")
                    .IsFixedLength();
            });

            modelBuilder.Entity<Habitacion>(entity =>
            {
                entity.HasKey(e => e.Numero)
                    .HasName("PK__HABITACI__7500EDCA110024E5");

                entity.ToTable("HABITACION");

                entity.Property(e => e.Numero).HasColumnName("NUMERO");

                entity.Property(e => e.Disponibilidad).HasColumnName("DISPONIBILIDAD");

                entity.Property(e => e.IdCategoria).HasColumnName("ID_CATEGORIA");

                entity.HasOne(d => d.IdCategoriaNavigation)
                    .WithMany(p => p.Habitacions)
                    .HasForeignKey(d => d.IdCategoria)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ID_CATEGORIA_HABITACION");
            });

            modelBuilder.Entity<Perfil>(entity =>
            {
                entity.ToTable("PERFIL");

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
                entity.HasKey(e => new { e.IdReserva, e.NumeroHabitacion, e.IdServicio })
                    .HasName("PK__RESERVA___5E5710599255A6D1");

                entity.ToTable("RESERVA_HABITACION_SERVICIO");

                entity.Property(e => e.IdReserva).HasColumnName("ID_RESERVA");

                entity.Property(e => e.NumeroHabitacion).HasColumnName("NUMERO_HABITACION");

                entity.Property(e => e.IdServicio).HasColumnName("ID_SERVICIO");

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

                entity.HasOne(d => d.NumeroHabitacionNavigation)
                    .WithMany(p => p.ReservaHabitacionServicios)
                    .HasForeignKey(d => d.NumeroHabitacion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HABITACION_RESERVA_HABITACION_SERVICIO");
            });

            modelBuilder.Entity<Servicio>(entity =>
            {
                entity.ToTable("SERVICIO");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Coste)
                    .HasColumnType("decimal(6, 2)")
                    .HasColumnName("COSTE");

                entity.Property(e => e.Descripcion)
                    .HasColumnType("text")
                    .HasColumnName("DESCRIPCION");

                entity.Property(e => e.Tipo)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("TIPO");
            });

            modelBuilder.Entity<TipoCama>(entity =>
            {
                entity.HasKey(e => new { e.IdCategoria, e.Tipo })
                    .HasName("PK__TIPO_CAM__60BAD50FA63098DD");

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

                entity.Property(e => e.IdPerfil).HasColumnName("ID_PERFIL");

                entity.HasOne(d => d.IdPerfilNavigation)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.IdPerfil)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PERFIL_USUARIO");
            });

            modelBuilder.Entity<VDetalleAlmacenamientoTabla>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_DETALLE_ALMACENAMIENTO_TABLAS");

                entity.Property(e => e.CampoIdentity)
                    .HasMaxLength(128)
                    .HasColumnName("CAMPO_IDENTITY");

                entity.Property(e => e.ClaveUnica).HasColumnName("CLAVE_UNICA");

                entity.Property(e => e.Columna)
                    .HasMaxLength(128)
                    .HasColumnName("COLUMNA");

                entity.Property(e => e.DatosFichero)
                    .HasMaxLength(128)
                    .HasColumnName("DATOS_FICHERO");

                entity.Property(e => e.DatosGrupoFicheros)
                    .HasMaxLength(128)
                    .HasColumnName("DATOS_GRUPO_FICHEROS");

                entity.Property(e => e.DatosRutaFichero)
                    .HasMaxLength(260)
                    .HasColumnName("DATOS_RUTA_FICHERO");

                entity.Property(e => e.Esquema)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnName("ESQUEMA");

                entity.Property(e => e.Filas).HasColumnName("FILAS");

                entity.Property(e => e.IdIndiceColumna).HasColumnName("ID_INDICE_COLUMNA");

                entity.Property(e => e.Incluida).HasColumnName("INCLUIDA");

                entity.Property(e => e.IndicesFichero)
                    .HasMaxLength(128)
                    .HasColumnName("INDICES_FICHERO");

                entity.Property(e => e.IndicesGrupoFichero)
                    .HasMaxLength(128)
                    .HasColumnName("INDICES_GRUPO_FICHERO");

                entity.Property(e => e.IndicesRutaFichero)
                    .HasMaxLength(260)
                    .HasColumnName("INDICES_RUTA_FICHERO");

                entity.Property(e => e.LibreMb).HasColumnName("LIBRE_MB");

                entity.Property(e => e.LobFichero)
                    .HasMaxLength(128)
                    .HasColumnName("LOB_FICHERO");

                entity.Property(e => e.LobGrupoFicheros)
                    .HasMaxLength(128)
                    .HasColumnName("LOB_GRUPO_FICHEROS");

                entity.Property(e => e.LobRutaFichero)
                    .HasMaxLength(260)
                    .HasColumnName("LOB_RUTA_FICHERO");

                entity.Property(e => e.NombreIndice)
                    .HasMaxLength(128)
                    .HasColumnName("NOMBRE_INDICE");

                entity.Property(e => e.NumColumna).HasColumnName("NUM_COLUMNA");

                entity.Property(e => e.Pk).HasColumnName("PK");

                entity.Property(e => e.PorcentajeLibre)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("PORCENTAJE_LIBRE");

                entity.Property(e => e.Tabla)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnName("TABLA");

                entity.Property(e => e.TipoIndice)
                    .HasMaxLength(60)
                    .HasColumnName("TIPO_INDICE")
                    .UseCollation("Latin1_General_CI_AS_KS_WS");

                entity.Property(e => e.TotalMb).HasColumnName("TOTAL_MB");
            });

            modelBuilder.Entity<VDetalleForeignKey>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_DETALLE_FOREIGN_KEYS");

                entity.Property(e => e.Columna)
                    .HasMaxLength(128)
                    .HasColumnName("COLUMNA");

                entity.Property(e => e.ColumnaReferencia)
                    .HasMaxLength(128)
                    .HasColumnName("COLUMNA_REFERENCIA");

                entity.Property(e => e.Esquema)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnName("ESQUEMA");

                entity.Property(e => e.EsquemaReferencia)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnName("ESQUEMA_REFERENCIA");

                entity.Property(e => e.NomFk)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnName("NOM_FK");

                entity.Property(e => e.Tabla)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnName("TABLA");

                entity.Property(e => e.TablaReferencia)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnName("TABLA_REFERENCIA");

                entity.Property(e => e.TipoActualizacionEnCascada).HasColumnName("TIPO_ACTUALIZACION_EN_CASCADA");

                entity.Property(e => e.TipoBorradoEnCascada).HasColumnName("TIPO_BORRADO_EN_CASCADA");
            });

            modelBuilder.Entity<VGrant>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_GRANT");

                entity.Property(e => e.Columna)
                    .HasMaxLength(128)
                    .HasColumnName("COLUMNA");

                entity.Property(e => e.Esquema)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnName("ESQUEMA");

                entity.Property(e => e.Grant)
                    .HasMaxLength(588)
                    .HasColumnName("GRANT")
                    .UseCollation("Latin1_General_CI_AS");

                entity.Property(e => e.NombreObjeto)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnName("NOMBRE_OBJETO");

                entity.Property(e => e.NombrePermiso)
                    .HasMaxLength(128)
                    .HasColumnName("NOMBRE_PERMISO")
                    .UseCollation("Latin1_General_CI_AS_KS_WS");

                entity.Property(e => e.Tipo)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasColumnName("TIPO")
                    .UseCollation("Latin1_General_CI_AS_KS_WS");

                entity.Property(e => e.TipoPermiso)
                    .HasMaxLength(60)
                    .HasColumnName("TIPO_PERMISO")
                    .UseCollation("Latin1_General_CI_AS_KS_WS");

                entity.Property(e => e.Usuario)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnName("USUARIO");
            });

            modelBuilder.Entity<VSql>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_SQL");

                entity.Property(e => e.Campo)
                    .HasMaxLength(128)
                    .HasColumnName("CAMPO");

                entity.Property(e => e.Clave)
                    .IsRequired()
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("CLAVE");

                entity.Property(e => e.Collate)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnName("COLLATE");

                entity.Property(e => e.DataSpaceId).HasColumnName("data_space_id");

                entity.Property(e => e.Defecto)
                    .IsRequired()
                    .HasMaxLength(4000)
                    .HasColumnName("DEFECTO");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPCION");

                entity.Property(e => e.Dimension).HasColumnName("DIMENSION");

                entity.Property(e => e.Filegroup)
                    .HasMaxLength(128)
                    .HasColumnName("FILEGROUP");

                entity.Property(e => e.Growth).HasColumnName("GROWTH");

                entity.Property(e => e.Grupo)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("GRUPO");

                entity.Property(e => e.GrupoFicheros)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnName("GRUPO_FICHEROS");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Indices).HasColumnName("INDICES");

                entity.Property(e => e.Kbytes).HasColumnName("KBYTES");

                entity.Property(e => e.Mbytes).HasColumnName("MBYTES");

                entity.Property(e => e.NFiles).HasColumnName("N_FILES");

                entity.Property(e => e.Name)
                    .HasMaxLength(128)
                    .HasColumnName("name");

                entity.Property(e => e.ObjectId).HasColumnName("object_id");

                entity.Property(e => e.PermiteNulos).HasColumnName("PERMITE_NULOS");

                entity.Property(e => e.Posicion).HasColumnName("POSICION");

                entity.Property(e => e.Tabla)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnName("TABLA");

                entity.Property(e => e.Tipo)
                    .HasMaxLength(128)
                    .HasColumnName("TIPO");

                entity.Property(e => e.Triggers)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("TRIGGERS");
            });

            modelBuilder.Entity<VSqlFichero>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("V_SQL_FICHEROS");

                entity.Property(e => e.Bbdd)
                    .HasMaxLength(128)
                    .HasColumnName("BBDD");

                entity.Property(e => e.Fichero)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnName("FICHERO")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.GrupoFichero)
                    .HasMaxLength(128)
                    .HasColumnName("GRUPO_FICHERO")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.LibreMb).HasColumnName("LIBRE_MB");

                entity.Property(e => e.PorcentajeLibre).HasColumnName("PORCENTAJE_LIBRE");

                entity.Property(e => e.RutaFichero)
                    .IsRequired()
                    .HasMaxLength(260)
                    .HasColumnName("RUTA_FICHERO");

                entity.Property(e => e.TotalMb).HasColumnName("TOTAL_MB");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
