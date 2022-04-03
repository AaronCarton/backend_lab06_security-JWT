namespace Sneakers.API.Configuration;

public class AuthenticationSettings
{
  public string? SecretKey { get; set; }
  public string? Issuer { get; set; }
  public string? Audience { get; set; }
}