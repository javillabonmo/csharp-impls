using ExpenseTracker.Enums;
using ExpenseTracker.Models;
using ExpenseTracker.Models.ExpenseTracker;

namespace ExpenseTracker.Services.ExpenseTracker;

/// <summary>
/// Interfaz para el servicio de gestión de gastos de Comestibles.
/// </summary>
public interface IGroceriesService
{
    Task<Groceries> CreateGroceries(Groceries groceries);

    Task<Groceries> UpdateGroceries(string id, Groceries groceries, Guid userId);

    Task<bool> DeleteGroceriesById(string id, Guid userId);

    Task<Groceries?> GetGroceriesById(string id, Guid userId);

    Task<PaginatedList<Groceries>> GetPaginatedGroceries(
        Guid userId,
        int pageIndex,
        int pageSize,
        string? searchBy = null,
        string? searchString = null,
        string? sortBy = null,
        SortOrderEnum sortOrder = SortOrderEnum.Ascending,
        DateTime? startDate = null,
        DateTime? endDate = null);
}
