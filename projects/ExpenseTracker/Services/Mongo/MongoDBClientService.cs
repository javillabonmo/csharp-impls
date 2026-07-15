using ExpenseTracker.Exceptions;
using MongoDB.Driver;

namespace ExpenseTracker.Services.Mongo;

public class MongoDBClientService : IMongoDBClientService
{
    private readonly IMongoDatabase _database;
    private readonly ILogger<MongoDBClientService> _logger;

    public MongoDBClientService(IMongoClient mongoClient, ILogger<MongoDBClientService> logger)
    {
        _database = mongoClient.GetDatabase("ExpenseTracker");
        _logger = logger;
    }

    public IMongoCollection<T> GetCollection<T>(string collectionName) where T : class
    {
        return _database.GetCollection<T>(collectionName);
    }

    public string HealthCheck()
    {
        try
        {
            _database.RunCommand<MongoDB.Bson.BsonDocument>(new MongoDB.Bson.BsonDocument("ping", 1));
            _logger.LogDebug("MongoDB ping successful");
            return "MongoDB is healthy";
        }
        catch (Exception ex)
        {
            ex.AddData("Database", "ExpenseTracker");
            ex.AddData("Operation", "HealthCheck");

            _logger.LogError(ex, "MongoDB health check failed");

            return $"MongoDB health check failed: {ex.Message}";
        }
    }
}

