using AutoMapper;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context.Entidades;
using Icp.HotelAPI.Controllers.ReservasController.DTO;
using Icp.HotelAPI.Servicios.ReservasService.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Icp.HotelAPI.Servicios.ReservasService
{
    public class ReservasService : IReservaService
    {
        private readonly FCT_ABR_11Context context;
        private readonly IMapper mapper;

        public ReservasService(
            FCT_ABR_11Context context,
            IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<List<ReservaDetallesCosteDTO>> ObtenerReservas()
        {
            var entidades = await context.Reservas
                .Include(x => x.ReservaHabitacionServicios)
                .ToListAsync();

            var dtos = mapper.Map<List<ReservaDetallesCosteDTO>>(entidades);
            return dtos;
        }

        public async Task<ReservaDetallesCosteDTO> ObtenerReservasPorId(int id)
        {
            var entidad = await context.Reservas
                .Include(x => x.ReservaHabitacionServicios)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entidad == null)
            {
                throw new InvalidOperationException("La reserva no existe");
            }

            var dto = mapper.Map<ReservaDetallesCosteDTO>(entidad);
            return dto;
        }

        public async Task<List<ReservaDetallesServicioDTO>> ObtenerReservasPorIdHabitacion(int id)
        {
            var entidades = await context.Reservas
                .Include(x => x.ReservaHabitacionServicios)
                .Where(r => r.ReservaHabitacionServicios.Any(rhs => rhs.IdHabitacion == id))
                .ToListAsync();

            if (entidades == null || entidades.Count == 0)
            {
                throw new InvalidOperationException("No hay reservas para la habitacion especificada");
            }

            var dtos = mapper.Map<List<ReservaDetallesServicioDTO>>(entidades);
            return dtos;
        }

        public async Task<List<ReservaDetallesCosteDTO>> ObtenerReservasPorIdServicio(int id)
        {
            var entidades = await context.Reservas
                .Include(x => x.ReservaHabitacionServicios)
                .Where(r => r.ReservaHabitacionServicios.Any(rhs => rhs.IdServicio == id))
                .ToListAsync();

            if (entidades == null || entidades.Count == 0)
            {
                throw new InvalidOperationException("No hay reservas para el servicio especificado");
            }

            var dtos = mapper.Map<List<ReservaDetallesCosteDTO>>(entidades);
            return dtos;
        }

        public async Task<ActionResult> CrearReserva(ReservaCreacionDetallesDTO reservaCreacionDetallesDTO)
        {
            if (await VerificarExistencia(reservaCreacionDetallesDTO))
            {
                throw new InvalidOperationException("Una o más habitaciones solicitadas ya están reservadas en las fechas indicadas.");
            }

            var entidad = mapper.Map<Reserva>(reservaCreacionDetallesDTO);

            entidad.CosteTotal = 0M;

            context.Add(entidad);
            await context.SaveChangesAsync();

            foreach (var rhs in reservaCreacionDetallesDTO.ReservaHabitacionServicios)
            {
                var nuevaReserva = new ReservaHabitacionServicio
                {
                    IdReserva = entidad.Id,
                    IdHabitacion = rhs.IdHabitacion,
                    IdServicio = rhs.IdServicio
                };

                context.ReservaHabitacionServicios.Add(nuevaReserva);
            }

            await context.SaveChangesAsync();

            // Calcular el CosteTotal de la reserva
            decimal costeTotal = await CalcularCosteTotalReserva(entidad.Id);
            entidad.CosteTotal = costeTotal;

            // Actualizar el CosteTotal en la entidad de reserva
            context.Update(entidad);
            await context.SaveChangesAsync();

            var entidadDTO = mapper.Map<ReservaDetallesCosteDTO>(entidad);
            return new CreatedAtRouteResult("obtenerReserva", new { id = entidadDTO.Id }, entidadDTO);
        }

        public async Task<bool> ActualizarReserva(int id, ReservaCreacionDetallesDTO reservaCreacionDetallesDTO)
        {
            if (await VerificarExistencia(reservaCreacionDetallesDTO))
            {
                throw new InvalidOperationException("Una o más habitaciones solicitadas ya están reservadas en las fechas indicadas.");
            }

            var reservaDB = await context.Reservas
                .Include(x => x.ReservaHabitacionServicios)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (reservaDB == null)
            {
                return false;
            }

            mapper.Map(reservaCreacionDetallesDTO, reservaDB);
            reservaDB.ReservaHabitacionServicios.Clear();

            foreach (var habitacionDTO in reservaCreacionDetallesDTO.ReservaHabitacionServicios)
            {
                var habitacion = new ReservaHabitacionServicio
                {
                    IdReserva = id,
                    IdHabitacion = habitacionDTO.IdHabitacion,
                    IdServicio = habitacionDTO.IdServicio
                };
                reservaDB.ReservaHabitacionServicios.Add(habitacion);
            }

            // Necesario guardar los cambios para que la funcion de calcular coste reciba el contexto nuevo
            await context.SaveChangesAsync();

            reservaDB.CosteTotal = await CalcularCosteTotalReserva(id);

            context.Entry(reservaDB).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CancelarReserva(int id)
        {
            var reserva = await context.Reservas.FindAsync(id);

            if (reserva == null || reserva.Cancelada)
            {
                return false;
            }

            reserva.Cancelada = true;
            var hoy = DateTime.Today;
            var dosDiasAntes = hoy.AddDays(2);

            if (reserva.FechaInicio <= dosDiasAntes)
            {
                reserva.CosteTotal *= 0.1M;
            }
            else
            {
                reserva.CosteTotal = 0M;
            }

            context.Entry(reserva).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CambiarCampoReserva(int id, [FromBody] JsonPatchDocument<ReservaCreacionDetallesDTO> patchDocument)
        {
            if (patchDocument == null)
            {
                throw new InvalidOperationException("Peticion incorrecta");
            }

            var reservaDB = await context.Reservas
                .Include(x => x.ReservaHabitacionServicios)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (reservaDB == null)
            {
                return false;
            }

            var reservaDTO = mapper.Map<ReservaCreacionDetallesDTO>(reservaDB);
            patchDocument.ApplyTo(reservaDTO);

            var actualizado = await ActualizarReserva(id, reservaDTO);

            return true;
        }

        public async Task<bool> BorrarReserva(int id)
        {
            var entidad = await context.Reservas.FirstOrDefaultAsync(x => x.Id == id);

            if (entidad == null)
            {
                return false;
            }

            var reservaHabitacionServicio = await context.ReservaHabitacionServicios
                .Where(tc => tc.IdReserva == id)
                .ToListAsync();

            context.ReservaHabitacionServicios.RemoveRange(reservaHabitacionServicio);
            context.Reservas.Remove(entidad);

            await context.SaveChangesAsync();
            return true;
        }

        private async Task<decimal> CalcularCosteTotalReserva(int reservaId)
        {
            var detallesReserva = await context.ReservaHabitacionServicios
                .Where(rhs => rhs.IdReserva == reservaId)
                .ToListAsync();

            decimal costeTotal = 0;

            var habitacionesProcesadas = new HashSet<int>();

            foreach (var detalleReserva in detallesReserva)
            {
                if (!habitacionesProcesadas.Contains(detalleReserva.IdHabitacion))
                {
                    var habitacion = await context.Habitaciones.FindAsync(detalleReserva.IdHabitacion);
                    if (habitacion != null)
                    {
                        var categoria = await context.Categorias.FindAsync(habitacion.IdCategoria);
                        if (categoria != null)
                        {
                            costeTotal += categoria.CosteNoche * CalcularNumeroNochesReserva(reservaId);
                        }
                    }
                    habitacionesProcesadas.Add(detalleReserva.IdHabitacion);
                }

                var servicio = await context.Servicios.FindAsync(detalleReserva.IdServicio);
                if (servicio != null)
                {
                    costeTotal += servicio.Coste;
                }
            }
            return costeTotal;
        }

        private int CalcularNumeroNochesReserva(int reservaId)
        {
            var reserva = context.Reservas.Find(reservaId);
            if (reserva != null)
            {
                return (reserva.FechaFin - reserva.FechaInicio).Days;
            }
            return 0;
        }

        private async Task<bool> VerificarExistencia(ReservaCreacionDetallesDTO reservaCreacionDetallesDTO)
        {
            var habitacionesSolicitadas = reservaCreacionDetallesDTO.ReservaHabitacionServicios.Select(hs => hs.IdHabitacion).ToList();

            var existe = await context.ReservaHabitacionServicios
                .Where(rhs => habitacionesSolicitadas.Contains(rhs.IdHabitacion) &&
                    (
                        (reservaCreacionDetallesDTO.FechaInicio >= rhs.IdReservaNavigation.FechaInicio &&
                        reservaCreacionDetallesDTO.FechaInicio < rhs.IdReservaNavigation.FechaFin) ||
                        (reservaCreacionDetallesDTO.FechaFin <= rhs.IdReservaNavigation.FechaFin &&
                        reservaCreacionDetallesDTO.FechaFin > rhs.IdReservaNavigation.FechaInicio) ||
                        (rhs.IdReservaNavigation.FechaInicio >= reservaCreacionDetallesDTO.FechaInicio &&
                        rhs.IdReservaNavigation.FechaInicio < reservaCreacionDetallesDTO.FechaFin) ||
                        (rhs.IdReservaNavigation.FechaFin <= reservaCreacionDetallesDTO.FechaFin &&
                        rhs.IdReservaNavigation.FechaFin > reservaCreacionDetallesDTO.FechaInicio)
                    ) && 
                      !rhs.IdReservaNavigation.Cancelada
                )
                .AnyAsync();

            return existe;
        }
    }
}

