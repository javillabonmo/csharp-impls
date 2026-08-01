using ExpenseTracker.Enums;
using ExpenseTracker.Exceptions;
using ExpenseTracker.Models;
using ExpenseTracker.Models.ExpenseTracker;
using ExpenseTracker.Services.Mongo;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ExpenseTracker.Services.ExpenseTracker;

public class GroceriesService : IGroceriesService
{
    private readonly IMongoCollection<Groceries> _collection;
    private readonly GroceriesFilterBuilder _filterBuilder;

    public GroceriesService(
        IMongoDBClientService mongoDbClientService,
        GroceriesFilterBuilder filterBuilder)
    {
        _collection = mongoDbClientService.GetCollection<Groceries>("Groceries");
        _filterBuilder = filterBuilder;
    }

    public async Task<Groceries> CreateGroceries(Groceries groceries)
    {
        groceries.Id = ObjectId.GenerateNewId().ToString();
        groceries.CreatedAt = DateTime.UtcNow;

        ValidationHelper.Validate(groceries);

        try
        {
            await _collection.InsertOneAsync(groceries);
        }
        catch (MongoException ex)
        {
            ex.AddData("Collection", "Groceries")
              .AddData("Operation", nameof(CreateGroceries))
              .AddData("UserId", groceries.UserId);
            throw;
        }

        return groceries;
    }

    public async Task<Groceries> UpdateGroceries(string id, Groceries groceries, Guid userId)
    {
        groceries.UpdatedAt = DateTime.UtcNow;
        ValidationHelper.Validate(groceries);

        try
        {
            var result = await _collection.ReplaceOneAsync(
                g => g.Id == id && g.UserId == userId,
                groceries);

            if (result.MatchedCount > 0)
                return groceries;

            throw new KeyNotFoundException(
                    $"Groceries with id '{id}' not found or does not belong to the user.")
                .AddData("GroceriesId", id)
                .AddData("UserId", userId);
        }
        catch (MongoException ex)
        {
            ex.AddData("Collection", "Groceries")
              .AddData("Operation", nameof(UpdateGroceries))
              .AddData("GroceriesId", id)
              .AddData("UserId", userId);
            throw;
        }
    }

    public async Task<bool> DeleteGroceriesById(string id, Guid userId)
    {
        var result = await _collection.DeleteOneAsync(
            g => g.Id == id && g.UserId == userId);
        return result.DeletedCount > 0;
    }

    public async Task<Groceries?> GetGroceriesById(string id, Guid userId)
    {
        return await _collection
            .Find(g => g.Id == id && g.UserId == userId)
            .FirstOrDefaultAsync();
    }

    public async Task<PaginatedList<Groceries>> GetPaginatedGroceries(
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

            return new PaginatedList<Groceries>
            {
                Items = itemsTask.Result,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = (int)countTask.Result,
            };
        }
        catch (MongoException ex)
        {
            ex.AddData("Collection", "Groceries")
              .AddData("Operation", nameof(GetPaginatedGroceries))
              .AddData("UserId", userId)
              .AddData("PageIndex", pageIndex)
              .AddData("PageSize", pageSize);
            throw;
        }
    }
}
