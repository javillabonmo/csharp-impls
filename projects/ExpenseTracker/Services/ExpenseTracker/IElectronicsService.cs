using ExpenseTracker.Enums;
using ExpenseTracker.Models;
using ExpenseTracker.Models.ExpenseTracker;

namespace ExpenseTracker.Services.ExpenseTracker;

/// <summary>
/// Interfaz para el servicio de gestión de gastos de Electrónicos.
/// </summary>
public interface IElectronicsService
{
    Task<Electronics> CreateElectronics(Electronics electronics);

    Task<Electronics> UpdateElectronics(string id, Electronics electronics, Guid userId);

    Task<bool> DeleteElectronicsById(string id, Guid userId);

    Task<Electronics?> GetElectronicsById(string id, Guid userId);

    Task<PaginatedList<Electronics>> GetPaginatedElectronics(
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
