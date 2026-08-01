using ExpenseTracker.Enums;
using ExpenseTracker.Models.ExpenseTracker;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ExpenseTracker.Services.ExpenseTracker;

/// <summary>
/// Construye filtros y definiciones de ordenamiento de MongoDB para consultas de Electronics.
/// </summary>
public class ElectronicsFilterBuilder
{
    private static readonly HashSet<string> SearchableFields =
    [
        nameof(Electronics.ProductType),
        nameof(Electronics.Brand),
        nameof(Electronics.Model),
        nameof(Electronics.StoreName),
        nameof(Electronics.Description),
        nameof(Electronics.Color),
        nameof(Electronics.Specifications),
    ];

    private static readonly HashSet<string> SortableFields =
    [
        nameof(Electronics.Amount),
        nameof(Electronics.Date),
        nameof(Electronics.ProductType),
        nameof(Electronics.Brand),
    ];

    public List<FilterDefinition<Electronics>> BuildFilters(
        Guid userId,
        string? searchBy = null,
        string? searchString = null,
        DateTime? startDate = null,
        DateTime? endDate = null)
    {
        var filters = new List<FilterDefinition<Electronics>>
    {
            Builders<Electronics>.Filter.Eq(e => e.UserId, userId),
    };

        AddDateFilter(filters, startDate, endDate);
        AddSearchFilter(filters, searchBy, searchString);

        return filters;
    }

    public FilterDefinition<Electronics> CombineFilters(List<FilterDefinition<Electronics>> filters)
    {
        return Builders<Electronics>.Filter.And(filters);
    }

    public SortDefinition<Electronics> BuildSortDefinition(
        string? sortBy,
        SortOrderEnum sortOrder)
    {
        var field = sortBy?.ToLowerInvariant();

        if (field is null || !SortableFields.Contains(Capitalize(field)))
    {
            return sortOrder == SortOrderEnum.Ascending
                ? Builders<Electronics>.Sort.Ascending(e => e.Date)
                : Builders<Electronics>.Sort.Descending(e => e.Date);
    }

        return field switch
    {
            "amount" => sortOrder == SortOrderEnum.Ascending
                ? Builders<Electronics>.Sort.Ascending(e => e.Amount)
                : Builders<Electronics>.Sort.Descending(e => e.Amount),
            "date" => sortOrder == SortOrderEnum.Ascending
                ? Builders<Electronics>.Sort.Ascending(e => e.Date)
                : Builders<Electronics>.Sort.Descending(e => e.Date),
            "producttype" => sortOrder == SortOrderEnum.Ascending
                ? Builders<Electronics>.Sort.Ascending(e => e.ProductType)
                : Builders<Electronics>.Sort.Descending(e => e.ProductType),
            "brand" => sortOrder == SortOrderEnum.Ascending
                ? Builders<Electronics>.Sort.Ascending(e => e.Brand)
                : Builders<Electronics>.Sort.Descending(e => e.Brand),
            _ => sortOrder == SortOrderEnum.Ascending
                ? Builders<Electronics>.Sort.Ascending(e => e.Date)
                : Builders<Electronics>.Sort.Descending(e => e.Date)
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
        List<FilterDefinition<Electronics>> filters,
        DateTime? startDate,
        DateTime? endDate)
    {
        if (!startDate.HasValue && !endDate.HasValue)
            return;

        var dateFilter = Builders<Electronics>.Filter.Gte(e => e.Date, startDate ?? DateTime.MinValue)
            & Builders<Electronics>.Filter.Lte(e => e.Date, endDate ?? DateTime.MaxValue);

        filters.Add(dateFilter);
    }

    private void AddSearchFilter(
        List<FilterDefinition<Electronics>> filters,
        string? searchBy,
        string? searchString)
    {
        if (string.IsNullOrWhiteSpace(searchBy) || string.IsNullOrWhiteSpace(searchString))
            return;

        var fieldName = Capitalize(searchBy);

        if (!SearchableFields.Contains(fieldName))
    {
            throw new ArgumentException(
                $"Campo de búsqueda inválido: '{searchBy}'. " +
                $"Campos permitidos: {string.Join(", ", SearchableFields)}");
    }

        var propertyFilter = fieldName switch
    {
            nameof(Electronics.ProductType) => Builders<Electronics>.Filter.Regex(e => e.ProductType, new BsonRegularExpression(searchString, "i")),
            nameof(Electronics.Brand) => Builders<Electronics>.Filter.Regex(e => e.Brand, new BsonRegularExpression(searchString, "i")),
            nameof(Electronics.Model) => Builders<Electronics>.Filter.Regex(e => e.Model, new BsonRegularExpression(searchString, "i")),
            nameof(Electronics.StoreName) => Builders<Electronics>.Filter.Regex(e => e.StoreName, new BsonRegularExpression(searchString, "i")),
            nameof(Electronics.Description) => Builders<Electronics>.Filter.Regex(e => e.Description, new BsonRegularExpression(searchString, "i")),
            nameof(Electronics.Color) => Builders<Electronics>.Filter.Regex(e => e.Color, new BsonRegularExpression(searchString, "i")),
            nameof(Electronics.Specifications) => Builders<Electronics>.Filter.Regex(e => e.Specifications, new BsonRegularExpression(searchString, "i")),
            _ => throw new ArgumentException($"Campo de búsqueda no soportado: '{searchBy}'")
    };

        filters.Add(propertyFilter);
    }
}
