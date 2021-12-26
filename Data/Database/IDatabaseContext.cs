using MongoDB.Driver;

namespace BlazorServerTest.Data.Database;

public interface IDatabaseContext {
    IMongoClient Client { get; }
    string ConnectionString { get; }
}