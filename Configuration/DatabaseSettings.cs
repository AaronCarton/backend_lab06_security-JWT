namespace Sneakers.API.Configuration;

public class DatabaseSettings
{
    public string? ConnectionString { get; set; }
    public string? DatabaseName { get; set; }
    public string? SneakerCollection { get; set; }
    public string? BrandsCollection { get; set; }
    public string? OccasionCollection { get; set; }
    public string? OrdersCollection { get; set; }
}