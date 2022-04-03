namespace Sneakers.API.Models;

public class Brand
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? BrandId { get; set; }
    public string? Name { get; set; }
}





