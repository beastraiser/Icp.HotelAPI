﻿using AutoMapper;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context.Entidades;
using Icp.HotelAPI.Controllers.HabitacionesController.DTO;
using Icp.HotelAPI.Controllers.UsuariosController.DTO;
using Icp.HotelAPI.Servicios.HabitacionesService.Interfaces;
using Icp.HotelAPI.ServiciosCompartidos.PaginacionDTO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Icp.HotelAPI.Controllers.HabitacionesController
{
    [ApiController]
    [Route("api/habitaciones")]
    public class HabitacionesController : CustomBaseController.CustomBaseController
    {
        private readonly FCT_ABR_11Context context;
        private readonly IMapper mapper;
        private readonly IHabitacionService habitacionService;

        public HabitacionesController(
            FCT_ABR_11Context context, 
            IMapper mapper,
            IHabitacionService habitacionService) 
            : base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
            this.habitacionService = habitacionService;
        }

        
        // Obtener todas las habitaciones
        [HttpGet]
        public async Task<ActionResult<List<HabitacionDTO>>> ObtenerHabitaciones()
        {
            return await Get<Habitacion, HabitacionDTO>();
        }

        // Obtener habitacion por {id}
        [HttpGet("{id}", Name = "obtenerHabitacion")]
        public async Task<ActionResult<HabitacionDTO>> ObtenerHabitacionesPorId(int id)
        {
            return await Get<Habitacion, HabitacionDTO>(id);
        }

        // Obtener habitacion por fecha-inicio, fecha-fin y maximo personas
        [HttpPost("fechas")]
        public async Task<ActionResult<List<HabitacionDTO>>> ObtenerHabitacionesDisponibles([FromBody] DisponibilidadRequestDTO disponibilidadRequestDTO)
        {
            var habitacionesDisponibles = await habitacionService.ObtenerHabitacionesDisponiblesAsync(disponibilidadRequestDTO);
            return Ok(habitacionesDisponibles);
        }

        // Obtener todas las habitaciones con paginación (10 resultados máximo por página)
        [HttpGet("paginacion")]
        public async Task<ActionResult<List<HabitacionDTO>>> Get([FromQuery] PaginacionDTO paginacionDTO)
        {
            return await Get<Habitacion, HabitacionDTO>(paginacionDTO);
        }

        // Filtro por categoria y disponibilidad = true con paginación
        [HttpGet("categoria")]
        public async Task<ActionResult<List<HabitacionDTO>>> Filtrar([FromQuery] FiltroHabitacionDTO filtroHabitacionDTO)
        {
            return await habitacionService.Filtrar(filtroHabitacionDTO);
        }

        // Introducir una nueva habitacion
        [HttpPost("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "ADMIN")]
        public async Task<ActionResult> CrearNuevaHabitacion([FromBody] HabitacionPatchDTO habitacionDTO, int id)
        {
            return await Post<HabitacionPatchDTO, Habitacion, HabitacionDTO>(habitacionDTO, "obtenerHabitacion", id);
        }

        // Cambiar datos habitacion por id
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "ADMIN")]
        public async Task<ActionResult> CambiarDatosHabitacion(int id, [FromBody] HabitacionPatchDTO habitacionPatchDTO)
        {
            try
            {
                var respuesta = await habitacionService.ActualizarHabitacion(id, habitacionPatchDTO);
                if (respuesta)
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

        // Cambiar disponibilidad/categoria
        [HttpPatch("{id}")]
        public async Task<ActionResult> CambiarCampoHabitacion(int id, JsonPatchDocument<HabitacionPatchDTO> patchDocument)
        {
            return await Patch<Habitacion, HabitacionPatchDTO>(id, patchDocument);
        }

        // Borrar habitacion por {id}
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "ADMIN")]
        public async Task<ActionResult> BorrarHabitacion(int id)
        {
            return await Delete<Habitacion>(id);
        }
    }
}
