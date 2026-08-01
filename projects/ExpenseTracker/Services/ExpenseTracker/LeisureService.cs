using ExpenseTracker.Enums;
using ExpenseTracker.Exceptions;
using ExpenseTracker.Models;
using ExpenseTracker.Models.ExpenseTracker;
using ExpenseTracker.Services.Mongo;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ExpenseTracker.Services.ExpenseTracker;

public class LeisureService : ILeisureService
{
    private readonly IMongoCollection<Leisure> _collection;
    private readonly LeisureFilterBuilder _filterBuilder;

    public LeisureService(
        IMongoDBClientService mongoDbClientService,
        LeisureFilterBuilder filterBuilder)
    {
        _collection = mongoDbClientService.GetCollection<Leisure>("Leisure");
        _filterBuilder = filterBuilder;
    }

    public async Task<Leisure> CreateLeisure(Leisure leisure)
    {
        leisure.Id = ObjectId.GenerateNewId().ToString();
        leisure.CreatedAt = DateTime.UtcNow;

        ValidationHelper.Validate(leisure);

        try
        {
            await _collection.InsertOneAsync(leisure);
        }
        catch (MongoException ex)
        {
            ex.AddData("Collection", "Leisure")
              .AddData("Operation", nameof(CreateLeisure))
              .AddData("UserId", leisure.UserId);
            throw;
        }

        return leisure;
    }

    public async Task<Leisure> UpdateLeisure(string id, Leisure leisure, Guid userId)
    {
        leisure.UpdatedAt = DateTime.UtcNow;
        ValidationHelper.Validate(leisure);

        try
        {
            var result = await _collection.ReplaceOneAsync(
                l => l.Id == id && l.UserId == userId,
                leisure);

            if (result.MatchedCount > 0)
                return leisure;

            throw new KeyNotFoundException(
                    $"Leisure with id '{id}' not found or does not belong to the user.")
                .AddData("LeisureId", id)
                .AddData("UserId", userId);
        }
        catch (MongoException ex)
        {
            ex.AddData("Collection", "Leisure")
              .AddData("Operation", nameof(UpdateLeisure))
              .AddData("LeisureId", id)
              .AddData("UserId", userId);
            throw;
        }
    }

    public async Task<bool> DeleteLeisureById(string id, Guid userId)
    {
        var result = await _collection.DeleteOneAsync(
            l => l.Id == id && l.UserId == userId);
        return result.DeletedCount > 0;
    }

    public async Task<Leisure?> GetLeisureById(string id, Guid userId)
    {
        return await _collection
            .Find(l => l.Id == id && l.UserId == userId)
            .FirstOrDefaultAsync();
    }

    public async Task<PaginatedList<Leisure>> GetPaginatedLeisures(
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

            return new PaginatedList<Leisure>
            {
                Items = itemsTask.Result,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = (int)countTask.Result,
            };
        }
        catch (MongoException ex)
        {
            ex.AddData("Collection", "Leisure")
              .AddData("Operation", nameof(GetPaginatedLeisures))
              .AddData("UserId", userId)
              .AddData("PageIndex", pageIndex)
              .AddData("PageSize", pageSize);
            throw;
        }
    }
}
