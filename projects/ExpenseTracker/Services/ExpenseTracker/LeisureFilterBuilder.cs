using ExpenseTracker.Enums;
using ExpenseTracker.Models.ExpenseTracker;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ExpenseTracker.Services.ExpenseTracker;

/// <summary>
/// Construye filtros y definiciones de ordenamiento de MongoDB para consultas de Leisure.
/// </summary>
public class LeisureFilterBuilder
{
    private static readonly HashSet<string> SearchableFields =
    [
        nameof(Leisure.ActivityType),
        nameof(Leisure.Location),
        nameof(Leisure.EventName),
        nameof(Leisure.Description),
        nameof(Leisure.ConfirmationNumber),
    ];

    private static readonly HashSet<string> SortableFields =
    [
        nameof(Leisure.Amount),
        nameof(Leisure.Date),
        nameof(Leisure.ActivityType),
        nameof(Leisure.SatisfactionRating),
    ];

    public List<FilterDefinition<Leisure>> BuildFilters(
        Guid userId,
        string? searchBy = null,
        string? searchString = null,
        DateTime? startDate = null,
        DateTime? endDate = null)
    {
        var filters = new List<FilterDefinition<Leisure>>
    {
            Builders<Leisure>.Filter.Eq(l => l.UserId, userId),
    };

        AddDateFilter(filters, startDate, endDate);
        AddSearchFilter(filters, searchBy, searchString);

        return filters;
    }

    public FilterDefinition<Leisure> CombineFilters(List<FilterDefinition<Leisure>> filters)
    {
        return Builders<Leisure>.Filter.And(filters);
    }

    public SortDefinition<Leisure> BuildSortDefinition(
        string? sortBy,
        SortOrderEnum sortOrder)
    {
        var field = sortBy?.ToLowerInvariant();

        if (field is null || !SortableFields.Contains(Capitalize(field)))
    {
            return sortOrder == SortOrderEnum.Ascending
                ? Builders<Leisure>.Sort.Ascending(l => l.Date)
                : Builders<Leisure>.Sort.Descending(l => l.Date);
    }

        return field switch
    {
            "amount" => sortOrder == SortOrderEnum.Ascending
                ? Builders<Leisure>.Sort.Ascending(l => l.Amount)
                : Builders<Leisure>.Sort.Descending(l => l.Amount),
            "date" => sortOrder == SortOrderEnum.Ascending
                ? Builders<Leisure>.Sort.Ascending(l => l.Date)
                : Builders<Leisure>.Sort.Descending(l => l.Date),
            "activitytype" => sortOrder == SortOrderEnum.Ascending
                ? Builders<Leisure>.Sort.Ascending(l => l.ActivityType)
                : Builders<Leisure>.Sort.Descending(l => l.ActivityType),
            "satisfactionrating" => sortOrder == SortOrderEnum.Ascending
                ? Builders<Leisure>.Sort.Ascending(l => l.SatisfactionRating)
                : Builders<Leisure>.Sort.Descending(l => l.SatisfactionRating),
            _ => sortOrder == SortOrderEnum.Ascending
                ? Builders<Leisure>.Sort.Ascending(l => l.Date)
                : Builders<Leisure>.Sort.Descending(l => l.Date)
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
        List<FilterDefinition<Leisure>> filters,
        DateTime? startDate,
        DateTime? endDate)
    {
        if (!startDate.HasValue && !endDate.HasValue)
            return;

        var dateFilter = Builders<Leisure>.Filter.Gte(l => l.Date, startDate ?? DateTime.MinValue)
            & Builders<Leisure>.Filter.Lte(l => l.Date, endDate ?? DateTime.MaxValue);

        filters.Add(dateFilter);
    }

    private void AddSearchFilter(
        List<FilterDefinition<Leisure>> filters,
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
            nameof(Leisure.ActivityType) => Builders<Leisure>.Filter.Regex(l => l.ActivityType, new BsonRegularExpression(searchString, "i")),
            nameof(Leisure.Location) => Builders<Leisure>.Filter.Regex(l => l.Location, new BsonRegularExpression(searchString, "i")),
            nameof(Leisure.EventName) => Builders<Leisure>.Filter.Regex(l => l.EventName, new BsonRegularExpression(searchString, "i")),
            nameof(Leisure.Description) => Builders<Leisure>.Filter.Regex(l => l.Description, new BsonRegularExpression(searchString, "i")),
            nameof(Leisure.ConfirmationNumber) => Builders<Leisure>.Filter.Regex(l => l.ConfirmationNumber, new BsonRegularExpression(searchString, "i")),
            _ => throw new ArgumentException($"Campo de búsqueda no soportado: '{searchBy}'")
    };

        filters.Add(propertyFilter);
    }
}
