namespace ExpenseTracker.Models
{
    /// <summary>
    /// Representa una lista paginada de elementos con metadatos de paginación.
    /// </summary>
    /// <typeparam name="T">Tipo de los elementos en la lista.</typeparam>
    public class PaginatedList<T>
    {
        /// <summary>
        /// Obtiene o establece la lista de elementos de la página actual.
        /// </summary>
        public List<T> Items { get; set; } = [];

        /// <summary>
        /// Obtiene o establece el índice de la página actual (basado en 1).
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// Obtiene o establece el tamaño de página (número de elementos por página).
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Obtiene o establece el número total de elementos en todas las páginas.
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Obtiene el número total de páginas.
        /// </summary>
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

        /// <summary>
        /// Obtiene un valor que indica si existe una página anterior.
        /// </summary>
        public bool HasPreviousPage => PageIndex > 1;

        /// <summary>
        /// Obtiene un valor que indica si existe una página siguiente.
        /// </summary>
        public bool HasNextPage => PageIndex < TotalPages;
    }
}
