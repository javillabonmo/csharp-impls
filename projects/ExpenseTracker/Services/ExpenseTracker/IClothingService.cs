using ExpenseTracker.Enums;
using ExpenseTracker.Models;
using ExpenseTracker.Models.ExpenseTracker;

namespace ExpenseTracker.Services.ExpenseTracker
{
    public interface IClothingService
    {
        Task<Clothing> CreateClothing(Clothing clothing);

        Task<Clothing> UpdateClothing(string id, Clothing clothing, Guid userId);

        Task<bool> DeleteClothingById(string id, Guid userId);

        Task<Clothing?> GetClothingById(string id, Guid userId);

        Task<PaginatedList<Clothing>> GetPaginatedClothings(
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
}
