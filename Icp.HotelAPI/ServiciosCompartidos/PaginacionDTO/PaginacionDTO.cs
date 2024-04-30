﻿namespace Icp.HotelAPI.ServiciosCompartidos.PaginacionDTO
{
    public class PaginacionDTO
    {
        public int Pagina { get; set; } = 1;

        private int cantidadRegistrosPorPagina = 10;
        private readonly int cantidadMaximaRegistrosPorPagina = 50;

        public int CantidadRegistrosPorPagina
        {
            get => cantidadRegistrosPorPagina;
            set
            {
                cantidadRegistrosPorPagina = value > cantidadMaximaRegistrosPorPagina ? cantidadMaximaRegistrosPorPagina : value;
            }
        }
    }
}
