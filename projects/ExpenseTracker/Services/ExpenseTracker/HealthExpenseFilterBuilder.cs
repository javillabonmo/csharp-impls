using ExpenseTracker.Enums;
using ExpenseTracker.Models.ExpenseTracker;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ExpenseTracker.Services.ExpenseTracker;

/// <summary>
/// Construye filtros y definiciones de ordenamiento de MongoDB para consultas de Health.
/// </summary>
public class HealthExpenseFilterBuilder
{
    private static readonly HashSet<string> SearchableFields =
    [
        nameof(Health.HealthType),
        nameof(Health.Provider),
        nameof(Health.SpecialistName),
        nameof(Health.Specialty),
        nameof(Health.MedicationName),
        nameof(Health.PatientName),
        nameof(Health.Description),
    ];

    private static readonly HashSet<string> SortableFields =
    [
        nameof(Health.Amount),
        nameof(Health.Date),
        nameof(Health.HealthType),
        nameof(Health.Provider),
    ];

    public List<FilterDefinition<Health>> BuildFilters(
        Guid userId,
        string? searchBy = null,
        string? searchString = null,
        DateTime? startDate = null,
        DateTime? endDate = null)
    {
        var filters = new List<FilterDefinition<Health>>
    {
            Builders<Health>.Filter.Eq(h => h.UserId, userId),
    };

        AddDateFilter(filters, startDate, endDate);
        AddSearchFilter(filters, searchBy, searchString);

        return filters;
    }

    public FilterDefinition<Health> CombineFilters(List<FilterDefinition<Health>> filters)
    {
        return Builders<Health>.Filter.And(filters);
    }

    public SortDefinition<Health> BuildSortDefinition(
        string? sortBy,
        SortOrderEnum sortOrder)
    {
        var field = sortBy?.ToLowerInvariant();

        if (field is null || !SortableFields.Contains(Capitalize(field)))
    {
            return sortOrder == SortOrderEnum.Ascending
                ? Builders<Health>.Sort.Ascending(h => h.Date)
                : Builders<Health>.Sort.Descending(h => h.Date);
    }

        return field switch
    {
            "amount" => sortOrder == SortOrderEnum.Ascending
                ? Builders<Health>.Sort.Ascending(h => h.Amount)
                : Builders<Health>.Sort.Descending(h => h.Amount),
            "date" => sortOrder == SortOrderEnum.Ascending
                ? Builders<Health>.Sort.Ascending(h => h.Date)
                : Builders<Health>.Sort.Descending(h => h.Date),
            "healthtype" => sortOrder == SortOrderEnum.Ascending
                ? Builders<Health>.Sort.Ascending(h => h.HealthType)
                : Builders<Health>.Sort.Descending(h => h.HealthType),
            "provider" => sortOrder == SortOrderEnum.Ascending
                ? Builders<Health>.Sort.Ascending(h => h.Provider)
                : Builders<Health>.Sort.Descending(h => h.Provider),
            _ => sortOrder == SortOrderEnum.Ascending
                ? Builders<Health>.Sort.Ascending(h => h.Date)
                : Builders<Health>.Sort.Descending(h => h.Date)
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
        List<FilterDefinition<Health>> filters,
        DateTime? startDate,
        DateTime? endDate)
    {
        if (!startDate.HasValue && !endDate.HasValue)
            return;

        var dateFilter = Builders<Health>.Filter.Gte(h => h.Date, startDate ?? DateTime.MinValue)
            & Builders<Health>.Filter.Lte(h => h.Date, endDate ?? DateTime.MaxValue);

        filters.Add(dateFilter);
    }

    private void AddSearchFilter(
        List<FilterDefinition<Health>> filters,
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
            nameof(Health.HealthType) => Builders<Health>.Filter.Regex(h => h.HealthType, new BsonRegularExpression(searchString, "i")),
            nameof(Health.Provider) => Builders<Health>.Filter.Regex(h => h.Provider, new BsonRegularExpression(searchString, "i")),
            nameof(Health.SpecialistName) => Builders<Health>.Filter.Regex(h => h.SpecialistName, new BsonRegularExpression(searchString, "i")),
            nameof(Health.Specialty) => Builders<Health>.Filter.Regex(h => h.Specialty, new BsonRegularExpression(searchString, "i")),
            nameof(Health.MedicationName) => Builders<Health>.Filter.Regex(h => h.MedicationName, new BsonRegularExpression(searchString, "i")),
            nameof(Health.PatientName) => Builders<Health>.Filter.Regex(h => h.PatientName, new BsonRegularExpression(searchString, "i")),
            nameof(Health.Description) => Builders<Health>.Filter.Regex(h => h.Description, new BsonRegularExpression(searchString, "i")),
            _ => throw new ArgumentException($"Campo de búsqueda no soportado: '{searchBy}'")
    };

        filters.Add(propertyFilter);
    }
}
