namespace Sneakers.API.Models;

public class Sneaker
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? SneakerId { get; set; }
    public string? Name { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public Brand? Brand { get; set; }
    public List<Occasion>? Occasions { get; set; }
}