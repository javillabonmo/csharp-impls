using MongoDB.Driver;

namespace ExpenseTracker.Services.Mongo
{
    public interface IMongoDBClientService
    {
        IMongoCollection<T> GetCollection<T>(string collectionName) where T : class;
        string HealthCheck();
    }
}
