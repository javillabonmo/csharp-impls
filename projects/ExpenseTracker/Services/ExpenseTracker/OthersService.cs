using ExpenseTracker.Enums;
using ExpenseTracker.Exceptions;
using ExpenseTracker.Models;
using ExpenseTracker.Models.ExpenseTracker;
using ExpenseTracker.Services.Mongo;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ExpenseTracker.Services.ExpenseTracker;

public class OthersService : IOthersService
{
    private readonly IMongoCollection<Others> _collection;
    private readonly OthersFilterBuilder _filterBuilder;

    public OthersService(
        IMongoDBClientService mongoDbClientService,
        OthersFilterBuilder filterBuilder)
    {
        _collection = mongoDbClientService.GetCollection<Others>("Others");
        _filterBuilder = filterBuilder;
    }

    public async Task<Others> CreateOthers(Others others)
    {
        others.Id = ObjectId.GenerateNewId().ToString();
        others.CreatedAt = DateTime.UtcNow;

        ValidationHelper.Validate(others);

        try
        {
            await _collection.InsertOneAsync(others);
        }
        catch (MongoException ex)
        {
            ex.AddData("Collection", "Others")
              .AddData("Operation", nameof(CreateOthers))
              .AddData("UserId", others.UserId);
            throw;
        }

        return others;
    }

    public async Task<Others> UpdateOthers(string id, Others others, Guid userId)
    {
        others.UpdatedAt = DateTime.UtcNow;
        ValidationHelper.Validate(others);

        try
        {
            var result = await _collection.ReplaceOneAsync(
                o => o.Id == id && o.UserId == userId,
                others);

            if (result.MatchedCount > 0)
                return others;

            throw new KeyNotFoundException(
                    $"Others expense with id '{id}' not found or does not belong to the user.")
                .AddData("OthersId", id)
                .AddData("UserId", userId);
        }
        catch (MongoException ex)
        {
            ex.AddData("Collection", "Others")
              .AddData("Operation", nameof(UpdateOthers))
              .AddData("OthersId", id)
              .AddData("UserId", userId);
            throw;
        }
    }

    public async Task<bool> DeleteOthersById(string id, Guid userId)
    {
        var result = await _collection.DeleteOneAsync(
            o => o.Id == id && o.UserId == userId);
        return result.DeletedCount > 0;
    }

    public async Task<Others?> GetOthersById(string id, Guid userId)
    {
        return await _collection
            .Find(o => o.Id == id && o.UserId == userId)
            .FirstOrDefaultAsync();
    }

    public async Task<PaginatedList<Others>> GetPaginatedOthers(
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

            return new PaginatedList<Others>
            {
                Items = itemsTask.Result,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = (int)countTask.Result,
            };
        }
        catch (MongoException ex)
        {
            ex.AddData("Collection", "Others")
              .AddData("Operation", nameof(GetPaginatedOthers))
              .AddData("UserId", userId)
              .AddData("PageIndex", pageIndex)
              .AddData("PageSize", pageSize);
            throw;
        }
    }
}
