using ExpenseTracker.Enums;
using ExpenseTracker.Exceptions;
using ExpenseTracker.Models;
using ExpenseTracker.Models.ExpenseTracker;
using ExpenseTracker.Services.Mongo;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ExpenseTracker.Services.ExpenseTracker;

public class HealthExpenseService : IHealthExpenseService
{
    private readonly IMongoCollection<Health> _collection;
    private readonly HealthExpenseFilterBuilder _filterBuilder;

    public HealthExpenseService(
        IMongoDBClientService mongoDbClientService,
        HealthExpenseFilterBuilder filterBuilder)
    {
        _collection = mongoDbClientService.GetCollection<Health>("Health");
        _filterBuilder = filterBuilder;
    }

    public async Task<Health> CreateHealthExpense(Health healthExpense)
    {
        healthExpense.Id = ObjectId.GenerateNewId().ToString();
        healthExpense.CreatedAt = DateTime.UtcNow;

        ValidationHelper.Validate(healthExpense);

        try
        {
            await _collection.InsertOneAsync(healthExpense);
        }
        catch (MongoException ex)
        {
            ex.AddData("Collection", "Health")
              .AddData("Operation", nameof(CreateHealthExpense))
              .AddData("UserId", healthExpense.UserId);
            throw;
        }

        return healthExpense;
    }

    public async Task<Health> UpdateHealthExpense(string id, Health healthExpense, Guid userId)
    {
        healthExpense.UpdatedAt = DateTime.UtcNow;
        ValidationHelper.Validate(healthExpense);

        try
        {
            var result = await _collection.ReplaceOneAsync(
                h => h.Id == id && h.UserId == userId,
                healthExpense);

            if (result.MatchedCount > 0)
                return healthExpense;

            throw new KeyNotFoundException(
                    $"Health expense with id '{id}' not found or does not belong to the user.")
                .AddData("HealthExpenseId", id)
                .AddData("UserId", userId);
        }
        catch (MongoException ex)
        {
            ex.AddData("Collection", "Health")
              .AddData("Operation", nameof(UpdateHealthExpense))
              .AddData("HealthExpenseId", id)
              .AddData("UserId", userId);
            throw;
        }
    }

    public async Task<bool> DeleteHealthExpenseById(string id, Guid userId)
    {
        var result = await _collection.DeleteOneAsync(
            h => h.Id == id && h.UserId == userId);
        return result.DeletedCount > 0;
    }

    public async Task<Health?> GetHealthExpenseById(string id, Guid userId)
    {
        return await _collection
            .Find(h => h.Id == id && h.UserId == userId)
            .FirstOrDefaultAsync();
    }

    public async Task<PaginatedList<Health>> GetPaginatedHealthExpenses(
        Guid userId,
        int pageIndex,
        int pageSize,
        string? searchBy = null,
        string? searchString = null,
        string? sortBy = null,
        SortOrderEnum sortOrder = SortOrderEnum.Ascending,
        DateTime? startDate = null,
        DateTime? endDate = null)
    {
        var filters = _filterBuilder.BuildFilters(userId, searchBy, searchString, startDate, endDate);
        var combinedFilter = _filterBuilder.CombineFilters(filters);

        var sortDefinition = _filterBuilder.BuildSortDefinition(sortBy, sortOrder);

        try
        {
            var countTask = _collection.CountDocumentsAsync(combinedFilter);
            var itemsTask = _collection
                .Find(combinedFilter)
                .Sort(sortDefinition)
                .Skip((pageIndex - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            await Task.WhenAll(countTask, itemsTask);

            return new PaginatedList<Health>
            {
                Items = itemsTask.Result,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = (int)countTask.Result,
            };
        }
        catch (MongoException ex)
        {
            ex.AddData("Collection", "Health")
              .AddData("Operation", nameof(GetPaginatedHealthExpenses))
              .AddData("UserId", userId)
              .AddData("PageIndex", pageIndex)
              .AddData("PageSize", pageSize);
            throw;
        }
    }
}
