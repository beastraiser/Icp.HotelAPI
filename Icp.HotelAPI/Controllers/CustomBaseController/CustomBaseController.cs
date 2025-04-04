﻿using AutoMapper;
using Icp.HotelAPI.BBDD.FCT_ABR_11Context;
using Icp.HotelAPI.Controllers.Interfaces;
using Icp.HotelAPI.ServiciosCompartidos.PaginacionDTO;
using Icp.HotelAPI.ServiciosCompartidos.PaginacionDTO.Helpers;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Icp.HotelAPI.Controllers.CustomBaseController
{
    public class CustomBaseController : ControllerBase
    {
        private readonly FCT_ABR_11Context context;
        private readonly IMapper mapper;

        public CustomBaseController(FCT_ABR_11Context context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        // Get todos los registros
        protected async Task<List<TDTO>> Get<TEntidad, TDTO>() 
            where TEntidad : class
        {
            var entidades = await context.Set<TEntidad>().AsNoTracking().ToListAsync();
            var dtos = mapper.Map<List<TDTO>>(entidades);
            return dtos;
        }

        // Get todos los registros por campo Id
        protected async Task<ActionResult<TDTO>> Get<TEntidad, TDTO>(int id) 
            where TEntidad : class, IId
        {
            var entidad = await context.Set<TEntidad>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (entidad == null)
            {
                return NotFound();
            }
            return mapper.Map<TDTO>(entidad);
        }

        // Get con paginacion
        protected async Task<List<TDTO>> Get<TEntidad, TDTO>(PaginacionDTO paginacionDTO) 
            where TEntidad : class
        {
            var habitacion = context.Set<TEntidad>()
                .AsQueryable();

            await HttpContext.InsertarParametrosPaginacion(habitacion, paginacionDTO.CantidadRegistrosPorPagina);

            var entidades = await habitacion.Paginar(paginacionDTO).ToListAsync();
            var dtos = mapper.Map<List<TDTO>>(entidades);
            return dtos;
        }

        // Post un nuevo registro con id no autoincrementado
        protected async Task<ActionResult> Post<TCreacion, TEntidad, TLectura> (TCreacion creacionDTO, string nombreRuta, int id) 
            where TEntidad : class, IId
        {
            var existe = await context.Set<TEntidad>().AnyAsync(x => x.Id == id);

            if (existe)
            {
                return BadRequest(new { Message = $"Ya existe un registro con el id: {id}" });
            }

            var entidad = mapper.Map<TEntidad>(creacionDTO);
            entidad.Id = id;
            context.Add(entidad);
            await context.SaveChangesAsync();
            var dtoLectura = mapper.Map<TLectura>(entidad);
            return new CreatedAtRouteResult(nombreRuta, dtoLectura);
        }
        // Post un nuevo registro con id autoincrementado
        protected async Task<ActionResult> Post<TCreacion, TEntidad, TLectura>(TCreacion creacionDTO, string nombreRuta, params string[] nombresCampos)
            where TEntidad : class
        {
            foreach( var nombreCampo in  nombresCampos )
            {
                var valorCampo = typeof(TCreacion).GetProperty(nombreCampo).GetValue(creacionDTO);

                var existe = await context.Set<TEntidad>().AnyAsync(e => EF.Property<object>(e, nombreCampo).Equals(valorCampo));

                if (existe)
                {
                    return BadRequest(new { Message = $"Ya existe un registro con el {nombreCampo}: {valorCampo}" });
                }
            }

            var entidad = mapper.Map<TEntidad>(creacionDTO);
            context.Add(entidad);
            await context.SaveChangesAsync();
            var dtoLectura = mapper.Map<TLectura>(entidad);
            return new CreatedAtRouteResult(nombreRuta, dtoLectura);
        }


        // Put un registro por id
        protected async Task<ActionResult> Put<TCreacion, TEntidad>(TCreacion creacionDTO, int id) 
            where TEntidad : class, IId
        {
            var entidad = mapper.Map<TEntidad>(creacionDTO);
            entidad.Id = id;
            context.Entry(entidad).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }

        // Patch registro por id
        protected async Task<ActionResult> Patch<TEntidad, TDTO>(int id, JsonPatchDocument<TDTO> patchDocument) 
            where TDTO : class
            where TEntidad: class, IId
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var entidadDB = await context.Set<TEntidad>().FirstOrDefaultAsync(x => x.Id == id);

            if (entidadDB == null)
            {
                return NotFound();
            }

            var entidadDTO = mapper.Map<TDTO>(entidadDB);
            patchDocument.ApplyTo(entidadDTO, ModelState);

            var esValido = TryValidateModel(entidadDTO);

            if (!esValido)
            {
                return BadRequest(ModelState);
            }

            mapper.Map(entidadDTO, entidadDB);

            await context.SaveChangesAsync();
            return NoContent();
        }

        //Delete un registro por id
        protected async Task<ActionResult> Delete<TEntidad>(int id) 
            where TEntidad : class, IId, new()
        {
            var existe = await context.Set<TEntidad>().AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new TEntidad() { Id = id });
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
