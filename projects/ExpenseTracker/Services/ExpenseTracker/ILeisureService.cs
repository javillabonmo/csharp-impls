using ExpenseTracker.Enums;
using ExpenseTracker.Models;
using ExpenseTracker.Models.ExpenseTracker;

namespace ExpenseTracker.Services.ExpenseTracker;

/// <summary>
/// Interfaz para el servicio de gestión de gastos de Ocio / Entretenimiento.
/// </summary>
public interface ILeisureService
{
    Task<Leisure> CreateLeisure(Leisure leisure);

    Task<Leisure> UpdateLeisure(string id, Leisure leisure, Guid userId);

    Task<bool> DeleteLeisureById(string id, Guid userId);

    Task<Leisure?> GetLeisureById(string id, Guid userId);

    Task<PaginatedList<Leisure>> GetPaginatedLeisures(
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
