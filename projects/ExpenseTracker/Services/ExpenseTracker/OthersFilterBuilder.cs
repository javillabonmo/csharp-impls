using ExpenseTracker.Enums;
using ExpenseTracker.Models.ExpenseTracker;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ExpenseTracker.Services.ExpenseTracker;

/// <summary>
/// Construye filtros y definiciones de ordenamiento de MongoDB para consultas de Others.
/// </summary>
public class OthersFilterBuilder
{
    private static readonly HashSet<string> SearchableFields =
    [
        nameof(Others.CustomCategory),
        nameof(Others.PaymentType),
        nameof(Others.VendorName),
        nameof(Others.TaxCategory),
        nameof(Others.Description),
    ];

    private static readonly HashSet<string> SortableFields =
    [
        nameof(Others.Amount),
        nameof(Others.Date),
        nameof(Others.CustomCategory),
        nameof(Others.Priority),
    ];

    public List<FilterDefinition<Others>> BuildFilters(
        Guid userId,
        string? searchBy = null,
        string? searchString = null,
        DateTime? startDate = null,
        DateTime? endDate = null)
    {
        var filters = new List<FilterDefinition<Others>>
    {
            Builders<Others>.Filter.Eq(o => o.UserId, userId),
    };

        AddDateFilter(filters, startDate, endDate);
        AddSearchFilter(filters, searchBy, searchString);

        return filters;
    }

    public FilterDefinition<Others> CombineFilters(List<FilterDefinition<Others>> filters)
    {
        return Builders<Others>.Filter.And(filters);
    }

    public SortDefinition<Others> BuildSortDefinition(
        string? sortBy,
        SortOrderEnum sortOrder)
    {
        var field = sortBy?.ToLowerInvariant();

        if (field is null || !SortableFields.Contains(Capitalize(field)))
    {
            return sortOrder == SortOrderEnum.Ascending
                ? Builders<Others>.Sort.Ascending(o => o.Date)
                : Builders<Others>.Sort.Descending(o => o.Date);
    }

        return field switch
    {
            "amount" => sortOrder == SortOrderEnum.Ascending
                ? Builders<Others>.Sort.Ascending(o => o.Amount)
                : Builders<Others>.Sort.Descending(o => o.Amount),
            "date" => sortOrder == SortOrderEnum.Ascending
                ? Builders<Others>.Sort.Ascending(o => o.Date)
                : Builders<Others>.Sort.Descending(o => o.Date),
            "customcategory" => sortOrder == SortOrderEnum.Ascending
                ? Builders<Others>.Sort.Ascending(o => o.CustomCategory)
                : Builders<Others>.Sort.Descending(o => o.CustomCategory),
            "priority" => sortOrder == SortOrderEnum.Ascending
                ? Builders<Others>.Sort.Ascending(o => o.Priority)
                : Builders<Others>.Sort.Descending(o => o.Priority),
            _ => sortOrder == SortOrderEnum.Ascending
                ? Builders<Others>.Sort.Ascending(o => o.Date)
                : Builders<Others>.Sort.Descending(o => o.Date)
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
        List<FilterDefinition<Others>> filters,
        DateTime? startDate,
        DateTime? endDate)
    {
        if (!startDate.HasValue && !endDate.HasValue)
            return;

        var dateFilter = Builders<Others>.Filter.Gte(o => o.Date, startDate ?? DateTime.MinValue)
            & Builders<Others>.Filter.Lte(o => o.Date, endDate ?? DateTime.MaxValue);

        filters.Add(dateFilter);
    }

    private void AddSearchFilter(
        List<FilterDefinition<Others>> filters,
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
            nameof(Others.CustomCategory) => Builders<Others>.Filter.Regex(o => o.CustomCategory, new BsonRegularExpression(searchString, "i")),
            nameof(Others.PaymentType) => Builders<Others>.Filter.Regex(o => o.PaymentType, new BsonRegularExpression(searchString, "i")),
            nameof(Others.VendorName) => Builders<Others>.Filter.Regex(o => o.VendorName, new BsonRegularExpression(searchString, "i")),
            nameof(Others.TaxCategory) => Builders<Others>.Filter.Regex(o => o.TaxCategory, new BsonRegularExpression(searchString, "i")),
            nameof(Others.Description) => Builders<Others>.Filter.Regex(o => o.Description, new BsonRegularExpression(searchString, "i")),
            _ => throw new ArgumentException($"Campo de búsqueda no soportado: '{searchBy}'")
    };

        filters.Add(propertyFilter);
    }
}
