﻿using AutoMapper;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context.Entidades;
using Icp.HotelAPI.Controllers.ReservasController.DTO;
using Icp.HotelAPI.Controllers.UsuariosController.DTO;
using Icp.HotelAPI.Servicios.ReservasService.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Icp.HotelAPI.Controllers.ReservasController
{
    [ApiController]
    [Route("api/reservas")]
    public class ReservasController : CustomBaseController.CustomBaseController
    {
        private readonly FCT_ABR_11Context context;
        private readonly IMapper mapper;
        private readonly IReservaService reservaService;

        public ReservasController(FCT_ABR_11Context context, IMapper mapper, IReservaService reservaService) : base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
            this.reservaService = reservaService;
        }

        //Obtener todas las reservas con sus habitaciones y servicios
        [HttpGet]
        public async Task<ActionResult<List<ReservaDetallesCosteDTO>>> ObtenerReservas()
        {
            return await reservaService.ObtenerReservas();
        }

        // Obtener reserva con habitaciones y servicios por id reserva
        [HttpGet("{id}", Name = "obtenerReserva")]
        public async Task<ActionResult<ReservaDetallesMostrarDTO>> ObtenerReservasPorId(int id)
        {
            return await reservaService.ObtenerReservasPorId(id);
        }

        // Obtener reserva con habitaciones y servicios por id usuario
        [HttpGet("usuario/{id}", Name = "obtenerReservaUs")]
        public async Task<ActionResult<List<ReservaDetallesMostrarDTO>>> ObtenerReservasPorIdUsuario(int id)
        {
            try
            {
                return await reservaService.ObtenerReservasPorIdUsuario(id);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { ex.Message });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Ocurrió un error inesperado. Por favor, intente de nuevo más tarde." });
            }
            
        }

        // Obtener reserva con habitaciones y servicios por id cliente
        [HttpGet("cliente/{id}", Name = "obtenerReservaCli")]
        public async Task<ActionResult<List<ReservaDetallesMostrarDTO>>> ObtenerReservasPorIdCliente(int id)
        {
            try
            {
                return await reservaService.ObtenerReservasPorIdCliente(id);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { ex.Message });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Ocurrió un error inesperado. Por favor, intente de nuevo más tarde." });
            }

            
        }

        // Obtener las reservas con servicios por id habitacion
        [HttpGet("habitacion/{id}", Name = "obtenerReservaHabitacion")]
        public async Task<ActionResult<List<ReservaDetallesServicioDTO>>> ObtenerReservasPorIdHabitacion(int id)
        {
            return await reservaService.ObtenerReservasPorIdHabitacion(id);
        }

        // Obtener las reservas con servicios por id servicio
        [HttpGet("servicio/{id}", Name = "obtenerReservaServicio")]
        public async Task<ActionResult<List<ReservaDetallesCosteDTO>>> ObtenerReservasPorIdServicio(int id)
        {
            return await reservaService.ObtenerReservasPorIdServicio(id);
        }

        // Pagar reserva
        [HttpGet("{id}/pagar", Name = "pagarReserva")]
        public async Task<ActionResult> PagarReserva(int id)
        {
            try
            {
                var pagado = await reservaService.PagarReserva(id);
                if (pagado)
                {
                    return Ok();
                }
                return BadRequest();
            }
            catch (InvalidOperationException ex)
            {
                return Unauthorized(new { ex.Message });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Ocurrió un error inesperado. Por favor, intente de nuevo más tarde." });
            }
        }

        // Agregar reserva con habitaciones y servicios y calcular precio
        [HttpPost]
        public async Task<ActionResult> CrearReserva([FromBody] ReservaCreacionDetallesDTO reservaCreacionDetallesDTO)
        {
            try
            {
                return await reservaService.CrearReserva(reservaCreacionDetallesDTO);
            }
            catch (InvalidOperationException ex)
            {
                return Unauthorized(new { ex.Message });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Ocurrió un error inesperado. Por favor, intente de nuevo más tarde." });
            }
            
        }

        // Cambiar datos reserva por id, incluida habitacion y servicios, y recalcular precio
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "LOGGED")]
        public async Task<ActionResult> ActualizarReserva(int id, [FromBody] ReservaCreacionDetallesDTO reservaCreacionDetallesDTO)
        {
            try
            {
                var actualizado = await reservaService.ActualizarReserva(id, reservaCreacionDetallesDTO);

                if (!actualizado)
                {
                    return BadRequest(new { Message = "La reserva no existe." });
                }
                    return Ok(new { Message = "La reserva ha sido actualizada correctamente." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { ex.Message });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Ocurrió un error inesperado. Por favor, intente de nuevo más tarde." });
            }
        }

        // Cancelar reserva
        [HttpPut("{id}/cancelar")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "LOGGED")]
        public async Task<ActionResult> CancelarReserva(int id)
        {
            var cancelado = await reservaService.CancelarReserva(id);

            if (cancelado)
            {
                return Ok(new { Message = "La reserva ha sido cancelada correctamente" });
                }
            else
            {
                return BadRequest(new {
                    Message = "La reserva ya ha sido cancelada anteriormente."});
            }
        }


        // Cambiar solamente un campo especifico y recalcular precio
        [HttpPatch("{id}")]
        public async Task<ActionResult> CambiarCampoReserva(int id, [FromBody] JsonPatchDocument<ReservaCreacionDetallesDTO> patchDocument)
        {
            var actualizado = await reservaService.CambiarCampoReserva(id, patchDocument);

            if (actualizado)
            {
                return Ok("La reserva ha sido cambiada correctamente.");
            }
            else
            {
                return BadRequest("La reserva no existe");
            }
        }

        // Borrar todos los datos de una reserva
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "ADMIN")]
        public async Task<ActionResult> BorrarReserva(int id)
        {
            var borrado = await reservaService.BorrarReserva(id);

            if (borrado)
            {
                return Ok("La reserva ha sido eliminada correctamente.");
            }
            else
            {
                return BadRequest("La reserva no existe");
            }
        }
    }
}
