namespace Sneakers.API.Repositories;

public interface IBrandRepository
{
    Task<List<Brand>> AddBrands(List<Brand> newBrands);
    Task<List<Brand>> GetAllBrands();
    Task<Brand> GetBrandById(string id);
    Task<Brand> UpdateBrand(Brand Brand);
}

public class BrandRepository : IBrandRepository
{
    private readonly IMongoContext _context;

    public BrandRepository(IMongoContext context)
    {
        _context = context;
    }

    public async Task<List<Brand>> AddBrands(List<Brand> newBrands)
    {
        await _context.BrandsCollection.InsertManyAsync(newBrands);
        return newBrands;
    }

    public async Task<List<Brand>> GetAllBrands() => await _context.BrandsCollection.Find(Brand => true).ToListAsync();

    public async Task<Brand> GetBrandById(string id) => await _context.BrandsCollection.Find(Brand => Brand.BrandId == id).FirstOrDefaultAsync();

    public async Task<Brand> UpdateBrand(Brand Brand)
    {
        try
        {
            var filter = Builders<Brand>.Filter.Eq(b => b.BrandId, Brand.BrandId);
            var update = Builders<Brand>.Update
                .Set(b => b.Name, Brand.Name);

            await _context.BrandsCollection.UpdateOneAsync(filter, update);
            return Brand;
        }
        catch (System.Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }
}