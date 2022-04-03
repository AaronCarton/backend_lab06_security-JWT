namespace Sneakers.API.Services;

public interface ISneakerService
{
    Task<Sneaker> AddSneakerAsync(Sneaker sneaker);
    Task<List<Brand>> GetBrandsAsync();
    Task<List<Occasion>> GetOccasionsAsync();
    Task<Sneaker> GetSneakerByIdAsync(string id);
    Task<List<Sneaker>> GetSneakersAsync();
    Task SetupData();
}

public class SneakerService : ISneakerService
{
    public readonly IOccasionRepository _occasionRepository;
    public readonly IBrandRepository _brandRepository;
    public readonly ISneakerRepository _sneakerRepository;

    public SneakerService(IOccasionRepository OccasionRepository, IBrandRepository brandRepository, ISneakerRepository sneakerRepository)
    {
        _occasionRepository = OccasionRepository;
        _brandRepository = brandRepository;
        _sneakerRepository = sneakerRepository;
    }
    public async Task<Sneaker> GetSneakerByIdAsync(string id) => await _sneakerRepository.GetSneakerById(id);

    public async Task<List<Sneaker>> GetSneakersAsync() => await _sneakerRepository.GetAllSneakers();

    public async Task<List<Brand>> GetBrandsAsync() => await _brandRepository.GetAllBrands();

    public async Task<List<Occasion>> GetOccasionsAsync() => await _occasionRepository.GetAllOccasions();

    public async Task<Sneaker> AddSneakerAsync(Sneaker sneaker)
    {
        await _sneakerRepository.AddSneakers(new List<Sneaker> { sneaker });
        return sneaker;
    }

    public async Task SetupData()
    {
        try
        {
            if (!(await _brandRepository.GetAllBrands()).Any())
                await _brandRepository.AddBrands(new List<Brand>() { new Brand() { Name = "ASICS" }, new Brand() { Name = "CONVERSE" }, new Brand() { Name = "JORDAN" }, new Brand() { Name = "PUMA" } });

            if (!(await _occasionRepository.GetAllOccasions()).Any())
                await _occasionRepository.AddOccasions(new List<Occasion>() { new Occasion() { Description = "Sports" }, new Occasion() { Description = "Casual" }, new Occasion() { Description = "Skate" }, new Occasion() { Description = "Diner" } });
        }
        catch (System.Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }
}