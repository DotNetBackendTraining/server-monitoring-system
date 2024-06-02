using MongoDB.Driver;
using ServerMonitoringSystem.Analyzer.Interfaces;
using ServerMonitoringSystem.Common.Models;

namespace ServerMonitoringSystem.Analyzer.Services;

public class MongoDbPersistenceService : IPersistenceService
{
    private readonly IMongoCollection<ServerStatisticsData> _collection;

    public MongoDbPersistenceService(IMongoClient mongoClient, string connectionString, string collectionName)
    {
        var databaseName = MongoUrl.Create(connectionString).DatabaseName;
        var database = mongoClient.GetDatabase(databaseName);
        _collection = database.GetCollection<ServerStatisticsData>(collectionName);
    }

    public async Task SaveServerStatisticsAsync(ServerStatisticsData data)
    {
        await _collection.InsertOneAsync(data);
    }
}