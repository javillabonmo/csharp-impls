using ExpenseTracker.Enums;
using ExpenseTracker.Exceptions;
using ExpenseTracker.Models;
using ExpenseTracker.Models.ExpenseTracker;
using ExpenseTracker.Services.Mongo;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ExpenseTracker.Services.ExpenseTracker;

public class UtilitiesService : IUtilitiesService
{
    private readonly IMongoCollection<Utilities> _collection;
    private readonly UtilitiesFilterBuilder _filterBuilder;

    public UtilitiesService(
        IMongoDBClientService mongoDbClientService,
        UtilitiesFilterBuilder filterBuilder)
    {
        _collection = mongoDbClientService.GetCollection<Utilities>("Utilities");
        _filterBuilder = filterBuilder;
    }

    public async Task<Utilities> CreateUtilities(Utilities utilities)
    {
        utilities.Id = ObjectId.GenerateNewId().ToString();
        utilities.CreatedAt = DateTime.UtcNow;

        ValidationHelper.Validate(utilities);

        try
        {
            await _collection.InsertOneAsync(utilities);
        }
        catch (MongoException ex)
        {
            ex.AddData("Collection", "Utilities")
              .AddData("Operation", nameof(CreateUtilities))
              .AddData("UserId", utilities.UserId);
            throw;
        }

        return utilities;
    }

    public async Task<Utilities> UpdateUtilities(string id, Utilities utilities, Guid userId)
    {
        utilities.UpdatedAt = DateTime.UtcNow;
        ValidationHelper.Validate(utilities);

        try
        {
            var result = await _collection.ReplaceOneAsync(
                u => u.Id == id && u.UserId == userId,
                utilities);

            if (result.MatchedCount > 0)
                return utilities;

            throw new KeyNotFoundException(
                    $"Utilities expense with id '{id}' not found or does not belong to the user.")
                .AddData("UtilitiesId", id)
                .AddData("UserId", userId);
        }
        catch (MongoException ex)
        {
            ex.AddData("Collection", "Utilities")
              .AddData("Operation", nameof(UpdateUtilities))
              .AddData("UtilitiesId", id)
              .AddData("UserId", userId);
            throw;
        }
    }

    public async Task<bool> DeleteUtilitiesById(string id, Guid userId)
    {
        var result = await _collection.DeleteOneAsync(
            u => u.Id == id && u.UserId == userId);
        return result.DeletedCount > 0;
    }

    public async Task<Utilities?> GetUtilitiesById(string id, Guid userId)
    {
        return await _collection
            .Find(u => u.Id == id && u.UserId == userId)
            .FirstOrDefaultAsync();
    }

    public async Task<PaginatedList<Utilities>> GetPaginatedUtilities(
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

            return new PaginatedList<Utilities>
            {
                Items = itemsTask.Result,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = (int)countTask.Result,
            };
        }
        catch (MongoException ex)
        {
            ex.AddData("Collection", "Utilities")
              .AddData("Operation", nameof(GetPaginatedUtilities))
              .AddData("UserId", userId)
              .AddData("PageIndex", pageIndex)
              .AddData("PageSize", pageSize);
            throw;
        }
    }
}
