using ExpenseTracker.Enums;
using ExpenseTracker.Models;
using ExpenseTracker.Models.ExpenseTracker;

namespace ExpenseTracker.Services.ExpenseTracker;

/// <summary>
/// Interfaz para el servicio de gestión de gastos de Salud.
/// </summary>
public interface IHealthExpenseService
{
    Task<Health> CreateHealthExpense(Health healthExpense);

    Task<Health> UpdateHealthExpense(string id, Health healthExpense, Guid userId);

    Task<bool> DeleteHealthExpenseById(string id, Guid userId);

    Task<Health?> GetHealthExpenseById(string id, Guid userId);

    Task<PaginatedList<Health>> GetPaginatedHealthExpenses(
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
