using ExpenseTracker.Exceptions;
using MongoDB.Driver;

namespace ExpenseTracker.Services.Mongo;

public class MongoDBClientService : IMongoDBClientService
{
    private readonly IMongoDatabase _database;

    public MongoDBClientService(IMongoClient mongoClient)
    {
        _database = mongoClient.GetDatabase("ExpenseTracker");
    }

    public IMongoCollection<T> GetCollection<T>(string collectionName)
        where T : class
    {
        return _database.GetCollection<T>(collectionName);
    }

    public string HealthCheck()
    {
        try
        {
            _database.RunCommand<MongoDB.Bson.BsonDocument>(new MongoDB.Bson.BsonDocument("ping", 1));
            return "MongoDB is healthy";
        }
        catch (Exception ex)
        {
            ex.AddData("Database", "ExpenseTracker");
            ex.AddData("Operation", "HealthCheck");
            return $"MongoDB health check failed: {ex.Message}";
        }
    }
}
