using ExpenseTracker.Enums;
using ExpenseTracker.Models.ExpenseTracker;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ExpenseTracker.Services.ExpenseTracker;

/// <summary>
/// Construye filtros y definiciones de ordenamiento de MongoDB para consultas de Utilities.
/// </summary>
public class UtilitiesFilterBuilder
{
    private static readonly HashSet<string> SearchableFields =
    [
        nameof(Utilities.ServiceType),
        nameof(Utilities.Provider),
        nameof(Utilities.BillingPeriod),
        nameof(Utilities.AccountNumber),
        nameof(Utilities.Description),
    ];

    private static readonly HashSet<string> SortableFields =
    [
        nameof(Utilities.Amount),
        nameof(Utilities.Date),
        nameof(Utilities.ServiceType),
        nameof(Utilities.Provider),
    ];

    public List<FilterDefinition<Utilities>> BuildFilters(
        Guid userId,
        string? searchBy = null,
        string? searchString = null,
        DateTime? startDate = null,
        DateTime? endDate = null)
    {
        var filters = new List<FilterDefinition<Utilities>>
    {
            Builders<Utilities>.Filter.Eq(u => u.UserId, userId),
    };

        AddDateFilter(filters, startDate, endDate);
        AddSearchFilter(filters, searchBy, searchString);

        return filters;
    }

    public FilterDefinition<Utilities> CombineFilters(List<FilterDefinition<Utilities>> filters)
    {
        return Builders<Utilities>.Filter.And(filters);
    }

    public SortDefinition<Utilities> BuildSortDefinition(
        string? sortBy,
        SortOrderEnum sortOrder)
    {
        var field = sortBy?.ToLowerInvariant();

        if (field is null || !SortableFields.Contains(Capitalize(field)))
    {
            return sortOrder == SortOrderEnum.Ascending
                ? Builders<Utilities>.Sort.Ascending(u => u.Date)
                : Builders<Utilities>.Sort.Descending(u => u.Date);
    }

        return field switch
    {
            "amount" => sortOrder == SortOrderEnum.Ascending
                ? Builders<Utilities>.Sort.Ascending(u => u.Amount)
                : Builders<Utilities>.Sort.Descending(u => u.Amount),
            "date" => sortOrder == SortOrderEnum.Ascending
                ? Builders<Utilities>.Sort.Ascending(u => u.Date)
                : Builders<Utilities>.Sort.Descending(u => u.Date),
            "servicetype" => sortOrder == SortOrderEnum.Ascending
                ? Builders<Utilities>.Sort.Ascending(u => u.ServiceType)
                : Builders<Utilities>.Sort.Descending(u => u.ServiceType),
            "provider" => sortOrder == SortOrderEnum.Ascending
                ? Builders<Utilities>.Sort.Ascending(u => u.Provider)
                : Builders<Utilities>.Sort.Descending(u => u.Provider),
            _ => sortOrder == SortOrderEnum.Ascending
                ? Builders<Utilities>.Sort.Ascending(u => u.Date)
                : Builders<Utilities>.Sort.Descending(u => u.Date)
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
        List<FilterDefinition<Utilities>> filters,
        DateTime? startDate,
        DateTime? endDate)
    {
        if (!startDate.HasValue && !endDate.HasValue)
            return;

        var dateFilter = Builders<Utilities>.Filter.Gte(u => u.Date, startDate ?? DateTime.MinValue)
            & Builders<Utilities>.Filter.Lte(u => u.Date, endDate ?? DateTime.MaxValue);

        filters.Add(dateFilter);
    }

    private void AddSearchFilter(
        List<FilterDefinition<Utilities>> filters,
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
            nameof(Utilities.ServiceType) => Builders<Utilities>.Filter.Regex(u => u.ServiceType, new BsonRegularExpression(searchString, "i")),
            nameof(Utilities.Provider) => Builders<Utilities>.Filter.Regex(u => u.Provider, new BsonRegularExpression(searchString, "i")),
            nameof(Utilities.BillingPeriod) => Builders<Utilities>.Filter.Regex(u => u.BillingPeriod, new BsonRegularExpression(searchString, "i")),
            nameof(Utilities.AccountNumber) => Builders<Utilities>.Filter.Regex(u => u.AccountNumber, new BsonRegularExpression(searchString, "i")),
            nameof(Utilities.Description) => Builders<Utilities>.Filter.Regex(u => u.Description, new BsonRegularExpression(searchString, "i")),
            _ => throw new ArgumentException($"Campo de búsqueda no soportado: '{searchBy}'")
    };

        filters.Add(propertyFilter);
    }
}
