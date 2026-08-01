using ExpenseTracker.Enums;
using ExpenseTracker.Exceptions;
using ExpenseTracker.Models;
using ExpenseTracker.Models.ExpenseTracker;
using ExpenseTracker.Services.Mongo;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ExpenseTracker.Services.ExpenseTracker;

public class ElectronicsService : IElectronicsService
{
    private readonly IMongoCollection<Electronics> _collection;
    private readonly ElectronicsFilterBuilder _filterBuilder;

    public ElectronicsService(
        IMongoDBClientService mongoDbClientService,
        ElectronicsFilterBuilder filterBuilder)
    {
        _collection = mongoDbClientService.GetCollection<Electronics>("Electronics");
        _filterBuilder = filterBuilder;
    }

    public async Task<Electronics> CreateElectronics(Electronics electronics)
    {
        electronics.Id = ObjectId.GenerateNewId().ToString();
        electronics.CreatedAt = DateTime.UtcNow;

        ValidationHelper.Validate(electronics);

        try
        {
            await _collection.InsertOneAsync(electronics);
        }
        catch (MongoException ex)
        {
            ex.AddData("Collection", "Electronics")
              .AddData("Operation", nameof(CreateElectronics))
              .AddData("UserId", electronics.UserId);
            throw;
        }

        return electronics;
    }

    public async Task<Electronics> UpdateElectronics(string id, Electronics electronics, Guid userId)
    {
        electronics.UpdatedAt = DateTime.UtcNow;
        ValidationHelper.Validate(electronics);

        try
        {
            var result = await _collection.ReplaceOneAsync(
                e => e.Id == id && e.UserId == userId,
                electronics);

            if (result.MatchedCount > 0)
                return electronics;

            throw new KeyNotFoundException(
                    $"Electronics with id '{id}' not found or does not belong to the user.")
                .AddData("ElectronicsId", id)
                .AddData("UserId", userId);
        }
        catch (MongoException ex)
        {
            ex.AddData("Collection", "Electronics")
              .AddData("Operation", nameof(UpdateElectronics))
              .AddData("ElectronicsId", id)
              .AddData("UserId", userId);
            throw;
        }
    }

    public async Task<bool> DeleteElectronicsById(string id, Guid userId)
    {
        var result = await _collection.DeleteOneAsync(
            e => e.Id == id && e.UserId == userId);
        return result.DeletedCount > 0;
    }

    public async Task<Electronics?> GetElectronicsById(string id, Guid userId)
    {
        return await _collection
            .Find(e => e.Id == id && e.UserId == userId)
            .FirstOrDefaultAsync();
    }

    public async Task<PaginatedList<Electronics>> GetPaginatedElectronics(
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

            return new PaginatedList<Electronics>
            {
                Items = itemsTask.Result,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = (int)countTask.Result,
            };
        }
        catch (MongoException ex)
        {
            ex.AddData("Collection", "Electronics")
              .AddData("Operation", nameof(GetPaginatedElectronics))
              .AddData("UserId", userId)
              .AddData("PageIndex", pageIndex)
              .AddData("PageSize", pageSize);
            throw;
        }
    }
}
