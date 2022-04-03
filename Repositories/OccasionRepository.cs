namespace Sneakers.API.Repositories;

public interface IOccasionRepository
{
    Task<List<Occasion>> AddOccasions(List<Occasion> newOccasions);
    Task<List<Occasion>> GetAllOccasions();
    Task<Occasion> GetOccasionById(string id);
    Task<Occasion> UpdateOccasion(Occasion Occasion);
}

public class OccasionRepository : IOccasionRepository
{
    private readonly IMongoContext _context;

    public OccasionRepository(IMongoContext context)
    {
        _context = context;
    }

    public async Task<List<Occasion>> AddOccasions(List<Occasion> newOccasions)
    {
        await _context.OccasionsCollection.InsertManyAsync(newOccasions);
        return newOccasions;
    }

    public async Task<List<Occasion>> GetAllOccasions() => await _context.OccasionsCollection.Find(Occasion => true).ToListAsync();

    public async Task<Occasion> GetOccasionById(string id) => await _context.OccasionsCollection.Find(Occasion => Occasion.OccasionId == id).FirstOrDefaultAsync();

    public async Task<Occasion> UpdateOccasion(Occasion Occasion)
    {
        try
        {
            var filter = Builders<Occasion>.Filter.Eq(b => b.OccasionId, Occasion.OccasionId);
            var update = Builders<Occasion>.Update
                .Set(b => b.Description, Occasion.Description);

            await _context.OccasionsCollection.UpdateOneAsync(filter, update);
            return Occasion;
        }
        catch (System.Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }
}