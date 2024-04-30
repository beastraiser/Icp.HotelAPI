using System;
using System.Collections.Generic;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

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

        public virtual DbSet<Categoria> Categoria { get; set; }
        public virtual DbSet<Cliente> Clientes { get; set; }
        public virtual DbSet<Habitacion> Habitacions { get; set; }
        public virtual DbSet<Perfil> Perfils { get; set; }
        public virtual DbSet<Permiso> Permisos { get; set; }
        public virtual DbSet<Reserva> Reservas { get; set; }
        public virtual DbSet<ReservaHabitacionServicio> ReservaHabitacionServicios { get; set; }
        public virtual DbSet<Servicio> Servicios { get; set; }
        public virtual DbSet<TipoCama> TipoCamas { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //            if (!optionsBuilder.IsConfigured)
            //            {
            //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
            //                optionsBuilder.UseSqlServer("Server=ps17; Database=FCT_ABR_11; Trusted_Connection=True;");
            //            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.ToTable("CATEGORIA");

                entity.HasIndex(e => e.Tipo, "UQ__CATEGORI__B6FCAAA2AD60B5FF")
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

                entity.HasIndex(e => e.Dni, "UQ__CLIENTE__C035B8DD172BBB9F")
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
                    .HasName("PK__HABITACI__7500EDCAFBE0B09B");

                entity.ToTable("HABITACION");

                entity.Property(e => e.Numero).HasColumnName("NUMERO");

                entity.Property(e => e.Categoria).HasColumnName("CATEGORIA");

                entity.Property(e => e.Disponibilidad).HasColumnName("DISPONIBILIDAD");

                entity.HasOne(d => d.CategoriaNavigation)
                    .WithMany(p => p.Habitacions)
                    .HasForeignKey(d => d.Categoria)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CATEGORIA_HABITACION");
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

            modelBuilder.Entity<Permiso>(entity =>
            {
                entity.HasKey(e => new { e.IdPerfil, e.Permiso1 })
                    .HasName("PK__PERMISO__E57DEA5D84332DCB");

                entity.ToTable("PERMISO");

                entity.Property(e => e.IdPerfil).HasColumnName("ID_PERFIL");

                entity.Property(e => e.Permiso1)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("PERMISO");

                entity.HasOne(d => d.IdPerfilNavigation)
                    .WithMany(p => p.Permisos)
                    .HasForeignKey(d => d.IdPerfil)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PERFIL_PERMISO");
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
                    .HasName("PK__RESERVA___5E571059DE43BC2E");

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
                entity.HasKey(e => new { e.NumeroHabitacion, e.TipoCama1 })
                    .HasName("PK__TIPO_CAM__6C04A7321D46CFD2");

                entity.ToTable("TIPO_CAMA");

                entity.Property(e => e.NumeroHabitacion).HasColumnName("NUMERO_HABITACION");

                entity.Property(e => e.TipoCama1)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("TIPO_CAMA");

                entity.HasOne(d => d.NumeroHabitacionNavigation)
                    .WithMany(p => p.TipoCamas)
                    .HasForeignKey(d => d.NumeroHabitacion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HABITACION_TIPO_CAMA");
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

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
