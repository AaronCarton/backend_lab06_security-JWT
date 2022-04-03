namespace Sneakers.API.Context;

public interface IMongoContext
{
    IMongoClient Client { get; }
    IMongoDatabase Database { get; }
    IMongoCollection<Sneaker> SneakersCollection { get; }
    IMongoCollection<Brand> BrandsCollection { get; }
    IMongoCollection<Occasion> OccasionsCollection { get; }
    IMongoCollection<Order> OrdersCollection { get; }
}

public class MongoContext : IMongoContext
{
    private readonly MongoClient _client;
    private readonly IMongoDatabase _database;
    private readonly DatabaseSettings _settings;

    public IMongoClient Client
    {
        get
        {
            return _client;
        }
    }
    public IMongoDatabase Database => _database;

    public MongoContext(IOptions<DatabaseSettings> dbOptions)
    {
        _settings = dbOptions.Value;
        _client = new MongoClient(_settings.ConnectionString);
        _database = _client.GetDatabase(_settings.DatabaseName);
    }

    public IMongoCollection<Sneaker> SneakersCollection
    {
        get
        {
            return _database.GetCollection<Sneaker>(_settings.SneakerCollection);
        }
    }

    public IMongoCollection<Brand> BrandsCollection
    {
        get
        {
            return _database.GetCollection<Brand>(_settings.BrandsCollection);
        }
    }

    public IMongoCollection<Occasion> OccasionsCollection
    {
        get
        {
            return _database.GetCollection<Occasion>(_settings.OccasionCollection);
        }
    }

    public IMongoCollection<Order> OrdersCollection
    {
        get
        {
            return _database.GetCollection<Order>(_settings.OrdersCollection);
        }
    }
}