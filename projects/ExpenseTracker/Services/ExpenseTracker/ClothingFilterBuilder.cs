using ExpenseTracker.Enums;
using ExpenseTracker.Models.ExpenseTracker;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ExpenseTracker.Services.ExpenseTracker
{
    /// <summary>
    /// Responsabilidad única: construir filtros y definiciones de ordenamiento
    /// de MongoDB para consultas de Clothing.
    /// </summary>
    public class ClothingFilterBuilder
    {
        private static readonly HashSet<string> SearchableFields =
        [
            nameof(Clothing.ItemType),
            nameof(Clothing.Brand),
            nameof(Clothing.Color),
            nameof(Clothing.Material),
            nameof(Clothing.StoreName),
            nameof(Clothing.Description),
            nameof(Clothing.Size),
            nameof(Clothing.Season),
        ];

        private static readonly HashSet<string> SortableFields =
        [
            nameof(Clothing.Amount),
            nameof(Clothing.Date),
            nameof(Clothing.ItemType),
            nameof(Clothing.Brand),
        ];

        /// <summary>
        /// Construye la lista completa de filtros combinando usuario, fechas y búsqueda de texto.
        /// </summary>
        public List<FilterDefinition<Clothing>> BuildFilters(
            Guid userId,
            string? searchBy = null,
            string? searchString = null,
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            var filters = new List<FilterDefinition<Clothing>>
            {
                Builders<Clothing>.Filter.Eq(c => c.UserId, userId),
            };

            AddDateFilter(filters, startDate, endDate);
            AddSearchFilter(filters, searchBy, searchString);

            return filters;
        }

        /// <summary>
        /// Combina una lista de filtros en un único FilterDefinition AND.
        /// </summary>
        public FilterDefinition<Clothing> CombineFilters(List<FilterDefinition<Clothing>> filters)
        {
            return Builders<Clothing>.Filter.And(filters);
        }

        /// <summary>
        /// Construye la definición de ordenamiento según el campo y dirección indicados.
        /// </summary>
        public SortDefinition<Clothing> BuildSortDefinition(
            string? sortBy,
            SortOrderEnum sortOrder)
        {
            var field = sortBy?.ToLowerInvariant();

            if (field is null || !SortableFields.Contains(Capitalize(field)))
            {
                // Orden por defecto: fecha
                return sortOrder == SortOrderEnum.Ascending
                    ? Builders<Clothing>.Sort.Ascending(c => c.Date)
                    : Builders<Clothing>.Sort.Descending(c => c.Date);
            }

            return field switch
            {
                "amount" => sortOrder == SortOrderEnum.Ascending
                    ? Builders<Clothing>.Sort.Ascending(c => c.Amount)
                    : Builders<Clothing>.Sort.Descending(c => c.Amount),
                "date" => sortOrder == SortOrderEnum.Ascending
                    ? Builders<Clothing>.Sort.Ascending(c => c.Date)
                    : Builders<Clothing>.Sort.Descending(c => c.Date),
                "itemtype" => sortOrder == SortOrderEnum.Ascending
                    ? Builders<Clothing>.Sort.Ascending(c => c.ItemType)
                    : Builders<Clothing>.Sort.Descending(c => c.ItemType),
                "brand" => sortOrder == SortOrderEnum.Ascending
                    ? Builders<Clothing>.Sort.Ascending(c => c.Brand)
                    : Builders<Clothing>.Sort.Descending(c => c.Brand),
                _ => sortOrder == SortOrderEnum.Ascending
                    ? Builders<Clothing>.Sort.Ascending(c => c.Date)
                    : Builders<Clothing>.Sort.Descending(c => c.Date)
            };
        }

        /// <summary>
        /// Capitaliza la primera letra para matchear con el nombre de la propiedad C#.
        /// </summary>
        private static string Capitalize(string value)
        {
            return string.IsNullOrEmpty(value)
                ? value
                : char.ToUpperInvariant(value[0]) + value[1..];
        }

        private void AddDateFilter(
            List<FilterDefinition<Clothing>> filters,
            DateTime? startDate,
            DateTime? endDate)
        {
            if (!startDate.HasValue && !endDate.HasValue)
                return;

            var dateFilter = Builders<Clothing>.Filter.Gte(c => c.Date, startDate ?? DateTime.MinValue)
                & Builders<Clothing>.Filter.Lte(c => c.Date, endDate ?? DateTime.MaxValue);

            filters.Add(dateFilter);
        }

        private void AddSearchFilter(
            List<FilterDefinition<Clothing>> filters,
            string? searchBy,
            string? searchString)
        {
            if (string.IsNullOrWhiteSpace(searchBy) || string.IsNullOrWhiteSpace(searchString))
            {
                return;
            }

            var fieldName = Capitalize(searchBy);

            if (!SearchableFields.Contains(fieldName))
            {
                throw new ArgumentException(
                    $"Campo de búsqueda inválido: '{searchBy}'. " +
                    $"Campos permitidos: {string.Join(", ", SearchableFields)}");
            }

            var propertyFilter = fieldName switch
            {
                nameof(Clothing.ItemType) => Builders<Clothing>.Filter.Regex(c => c.ItemType, new BsonRegularExpression(searchString, "i")),
                nameof(Clothing.Brand) => Builders<Clothing>.Filter.Regex(c => c.Brand, new BsonRegularExpression(searchString, "i")),
                nameof(Clothing.Color) => Builders<Clothing>.Filter.Regex(c => c.Color, new BsonRegularExpression(searchString, "i")),
                nameof(Clothing.Material) => Builders<Clothing>.Filter.Regex(c => c.Material, new BsonRegularExpression(searchString, "i")),
                nameof(Clothing.StoreName) => Builders<Clothing>.Filter.Regex(c => c.StoreName, new BsonRegularExpression(searchString, "i")),
                nameof(Clothing.Description) => Builders<Clothing>.Filter.Regex(c => c.Description, new BsonRegularExpression(searchString, "i")),
                nameof(Clothing.Size) => Builders<Clothing>.Filter.Regex(c => c.Size, new BsonRegularExpression(searchString, "i")),
                nameof(Clothing.Season) => Builders<Clothing>.Filter.Regex(c => c.Season, new BsonRegularExpression(searchString, "i")),
                _ => throw new ArgumentException($"Campo de búsqueda no soportado: '{searchBy}'")
            };

            filters.Add(propertyFilter);
        }
    }
}
