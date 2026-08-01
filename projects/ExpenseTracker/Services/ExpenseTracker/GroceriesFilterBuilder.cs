using ExpenseTracker.Enums;
using ExpenseTracker.Models.ExpenseTracker;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ExpenseTracker.Services.ExpenseTracker;

/// <summary>
/// Construye filtros y definiciones de ordenamiento de MongoDB para consultas de Groceries.
/// </summary>
public class GroceriesFilterBuilder
{
    private static readonly HashSet<string> SearchableFields =
    [
        nameof(Groceries.StoreName),
        nameof(Groceries.GroceryCategory),
        nameof(Groceries.Brand),
        nameof(Groceries.Description),
        nameof(Groceries.ReceiptNumber),
    ];

    private static readonly HashSet<string> SortableFields =
    [
        nameof(Groceries.Amount),
        nameof(Groceries.Date),
        nameof(Groceries.StoreName),
        nameof(Groceries.GroceryCategory),
    ];

    public List<FilterDefinition<Groceries>> BuildFilters(
        Guid userId,
        string? searchBy = null,
        string? searchString = null,
        DateTime? startDate = null,
        DateTime? endDate = null)
    {
        var filters = new List<FilterDefinition<Groceries>>
    {
            Builders<Groceries>.Filter.Eq(g => g.UserId, userId),
    };

        AddDateFilter(filters, startDate, endDate);
        AddSearchFilter(filters, searchBy, searchString);

        return filters;
    }

    public FilterDefinition<Groceries> CombineFilters(List<FilterDefinition<Groceries>> filters)
    {
        return Builders<Groceries>.Filter.And(filters);
    }

    public SortDefinition<Groceries> BuildSortDefinition(
        string? sortBy,
        SortOrderEnum sortOrder)
    {
        var field = sortBy?.ToLowerInvariant();

        if (field is null || !SortableFields.Contains(Capitalize(field)))
    {
            return sortOrder == SortOrderEnum.Ascending
                ? Builders<Groceries>.Sort.Ascending(g => g.Date)
                : Builders<Groceries>.Sort.Descending(g => g.Date);
    }

        return field switch
    {
            "amount" => sortOrder == SortOrderEnum.Ascending
                ? Builders<Groceries>.Sort.Ascending(g => g.Amount)
                : Builders<Groceries>.Sort.Descending(g => g.Amount),
            "date" => sortOrder == SortOrderEnum.Ascending
                ? Builders<Groceries>.Sort.Ascending(g => g.Date)
                : Builders<Groceries>.Sort.Descending(g => g.Date),
            "storename" => sortOrder == SortOrderEnum.Ascending
                ? Builders<Groceries>.Sort.Ascending(g => g.StoreName)
                : Builders<Groceries>.Sort.Descending(g => g.StoreName),
            "grocerycategory" => sortOrder == SortOrderEnum.Ascending
                ? Builders<Groceries>.Sort.Ascending(g => g.GroceryCategory)
                : Builders<Groceries>.Sort.Descending(g => g.GroceryCategory),
            _ => sortOrder == SortOrderEnum.Ascending
                ? Builders<Groceries>.Sort.Ascending(g => g.Date)
                : Builders<Groceries>.Sort.Descending(g => g.Date)
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
        List<FilterDefinition<Groceries>> filters,
        DateTime? startDate,
        DateTime? endDate)
    {
        if (!startDate.HasValue && !endDate.HasValue)
            return;

        var dateFilter = Builders<Groceries>.Filter.Gte(g => g.Date, startDate ?? DateTime.MinValue)
            & Builders<Groceries>.Filter.Lte(g => g.Date, endDate ?? DateTime.MaxValue);

        filters.Add(dateFilter);
    }

    private void AddSearchFilter(
        List<FilterDefinition<Groceries>> filters,
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
            nameof(Groceries.StoreName) => Builders<Groceries>.Filter.Regex(g => g.StoreName, new BsonRegularExpression(searchString, "i")),
            nameof(Groceries.GroceryCategory) => Builders<Groceries>.Filter.Regex(g => g.GroceryCategory, new BsonRegularExpression(searchString, "i")),
            nameof(Groceries.Brand) => Builders<Groceries>.Filter.Regex(g => g.Brand, new BsonRegularExpression(searchString, "i")),
            nameof(Groceries.Description) => Builders<Groceries>.Filter.Regex(g => g.Description, new BsonRegularExpression(searchString, "i")),
            nameof(Groceries.ReceiptNumber) => Builders<Groceries>.Filter.Regex(g => g.ReceiptNumber, new BsonRegularExpression(searchString, "i")),
            _ => throw new ArgumentException($"Campo de búsqueda no soportado: '{searchBy}'")
    };

        filters.Add(propertyFilter);
    }
}
