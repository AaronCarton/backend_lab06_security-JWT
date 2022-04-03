namespace Sneakers.API.Repositories;

public interface ISneakerRepository
{
    Task<List<Sneaker>> AddSneakers(List<Sneaker> newSneakers);
    Task<List<Sneaker>> GetAllSneakers();
    Task<Sneaker> GetSneakerById(string id);
    Task<Sneaker> UpdateSneaker(Sneaker Sneaker);
}

public class SneakerRepository : ISneakerRepository
{
    private readonly IMongoContext _context;

    public SneakerRepository(IMongoContext context)
    {
        _context = context;
    }

    public async Task<List<Sneaker>> AddSneakers(List<Sneaker> newSneakers)
    {
        await _context.SneakersCollection.InsertManyAsync(newSneakers);
        return newSneakers;
    }

    public async Task<List<Sneaker>> GetAllSneakers() => await _context.SneakersCollection.Find(Sneaker => true).ToListAsync();

    public async Task<Sneaker> GetSneakerById(string id) => await _context.SneakersCollection.Find(Sneaker => Sneaker.SneakerId == id).FirstOrDefaultAsync();

    public async Task<Sneaker> UpdateSneaker(Sneaker Sneaker)
    {
        try
        {
            var filter = Builders<Sneaker>.Filter.Eq(b => b.SneakerId, Sneaker.SneakerId);
            var update = Builders<Sneaker>.Update
                .Set(b => b.Name, Sneaker.Name)
                .Set(b => b.Price, Sneaker.Price)
                .Set(b => b.Stock, Sneaker.Stock)
                .Set(b => b.Occasions, Sneaker.Occasions)
                .Set(b => b.Brand, Sneaker.Brand);

            await _context.SneakersCollection.UpdateOneAsync(filter, update);
            return Sneaker;
        }
        catch (System.Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }
}