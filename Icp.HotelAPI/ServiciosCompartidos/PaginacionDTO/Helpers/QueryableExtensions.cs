namespace Icp.HotelAPI.ServiciosCompartidos.PaginacionDTO.Helpers
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Paginar<T>(this IQueryable<T> queryable, PaginacionDTO paginacionDTO)
        {
            return queryable
                // Salta el (nº de pagina actual - 1) * cantidad de registros por pagina -> ej: si la página es 1, se salta 0 * 10 registros = 0
                .Skip((paginacionDTO.Pagina - 1) * paginacionDTO.CantidadRegistrosPorPagina)
                .Take(paginacionDTO.CantidadRegistrosPorPagina);
        }
    }
}
