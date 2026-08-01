using ExpenseTracker.Enums;
using ExpenseTracker.Models;
using ExpenseTracker.Models.ExpenseTracker;

namespace ExpenseTracker.Services.ExpenseTracker;

/// <summary>
/// Interfaz para el servicio de gestión de gastos de Otros / Varios.
/// </summary>
public interface IOthersService
{
    Task<Others> CreateOthers(Others others);

    Task<Others> UpdateOthers(string id, Others others, Guid userId);

    Task<bool> DeleteOthersById(string id, Guid userId);

    Task<Others?> GetOthersById(string id, Guid userId);

    Task<PaginatedList<Others>> GetPaginatedOthers(
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
