using ExpenseTracker.Enums;
using ExpenseTracker.Exceptions;
using ExpenseTracker.Models;
using ExpenseTracker.Models.ExpenseTracker;
using ExpenseTracker.Services.Mongo;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ExpenseTracker.Services.ExpenseTracker;

public class ClothingService : IClothingService
{
    private readonly IMongoCollection<Clothing> _collection;
    private readonly ClothingFilterBuilder _filterBuilder;
    private readonly ILogger<ClothingService> _logger;

    public ClothingService(
        IMongoDBClientService mongoDbClientService,
        ClothingFilterBuilder filterBuilder,
        ILogger<ClothingService> logger)
    {
        _collection = mongoDbClientService.GetCollection<Clothing>("Clothing");
        _filterBuilder = filterBuilder;
        _logger = logger;
    }

    public async Task<Clothing> CreateClothing(Clothing clothing)
    {
        clothing.Id = ObjectId.GenerateNewId().ToString();
        clothing.CreatedAt = DateTime.UtcNow;

        ValidationHelper.Validate(clothing);

        await _collection.InsertOneAsync(clothing);
        return clothing;
    }

    public async Task<Clothing> UpdateClothing(string id, Clothing clothing, Guid userId)
    {
        clothing.UpdatedAt = DateTime.UtcNow;
        ValidationHelper.Validate(clothing);

        var result = await _collection.ReplaceOneAsync(
            c => c.Id == id && c.UserId == userId,
            clothing);

        if (result.MatchedCount > 0)
            return clothing;

        throw new KeyNotFoundException(
                $"Clothing with id '{id}' not found or does not belong to the user.")
            .AddData("ClothingId", id)
            .AddData("UserId", userId);
    }

    public async Task<bool> DeleteClothingById(string id, Guid userId)
    {
        var result = await _collection.DeleteOneAsync(
            c => c.Id == id && c.UserId == userId);
        return result.DeletedCount > 0;
    }

    public async Task<Clothing?> GetClothingById(string id, Guid userId)
    {
        return await _collection
            .Find(c => c.Id == id && c.UserId == userId)
            .FirstOrDefaultAsync();
    }

    public async Task<PaginatedList<Clothing>> GetPaginatedClothings(
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

        var countTask = _collection.CountDocumentsAsync(combinedFilter);
        var itemsTask = _collection
            .Find(combinedFilter)
            .Sort(sortDefinition)
            .Skip((pageIndex - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync();

        await Task.WhenAll(countTask, itemsTask);

        return new PaginatedList<Clothing>
        {
            Items = itemsTask.Result,
            PageIndex = pageIndex,
            PageSize = pageSize,
            TotalCount = (int)countTask.Result
        };
    }
}
