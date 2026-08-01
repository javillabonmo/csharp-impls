using ExpenseTracker.Enums;
using ExpenseTracker.Models;
using ExpenseTracker.Models.ExpenseTracker;

namespace ExpenseTracker.Services.ExpenseTracker;

/// <summary>
/// Interfaz para el servicio de gestión de gastos de Servicios / Utilidades.
/// </summary>
public interface IUtilitiesService
{
    Task<Utilities> CreateUtilities(Utilities utilities);

    Task<Utilities> UpdateUtilities(string id, Utilities utilities, Guid userId);

    Task<bool> DeleteUtilitiesById(string id, Guid userId);

    Task<Utilities?> GetUtilitiesById(string id, Guid userId);

    Task<PaginatedList<Utilities>> GetPaginatedUtilities(
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
