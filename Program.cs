
var builder = WebApplication.CreateBuilder(args);

// load JTW config
builder.Services.Configure<AuthenticationSettings>(builder.Configuration.GetSection("AuthenticationSettings"));

// configure database
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("MongoConnection"));
builder.Services.AddTransient<IMongoContext, MongoContext>();

// add repositories
builder.Services.AddTransient<ISneakerRepository, SneakerRepository>();
builder.Services.AddTransient<IBrandRepository, BrandRepository>();
builder.Services.AddTransient<IOccasionRepository, OccasionRepository>();
// add services
builder.Services.AddTransient<ISneakerService, SneakerService>();
builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();
// add validators
builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<SneakerValidator>());

// // add bearer auth
// builder.Services.AddAuthentication("Bearer").AddJwtBearer(options =>
// {
//   options.TokenValidationParameters = new TokenValidationParameters
//   {
//     ValidateIssuer = true,
//     ValidateAudience = true,
//     ValidateLifetime = true,
//     ValidateIssuerSigningKey = true,
//     ValidIssuer = builder.Configuration.GetSection("AuthenticationSettings").Get<AuthenticationSettings>().Issuer,
//     ValidAudience = builder.Configuration.GetSection("AuthenticationSettings").Get<AuthenticationSettings>().Audience,
//     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AuthenticationSettings").Get<AuthenticationSettings>().SecretKey))
//   };
// });

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGet("/setup", (ISneakerService service) => service.SetupData());

// Get data
app.MapGet("/brands", (ISneakerService service) => service.GetBrandsAsync());
app.MapGet("/occasions", (ISneakerService service) => service.GetOccasionsAsync());
app.MapGet("/sneakers", (ISneakerService service) => service.GetSneakersAsync());
app.MapGet("/sneakers/{id}", (ISneakerService service, string id) => service.GetSneakerByIdAsync);

// Post data
app.MapPost("/sneakers", (IValidator<Sneaker> validator, ISneakerService sneakerService, Sneaker sneaker) =>
{
  var validatorResults = validator.Validate(sneaker);
  if (!validatorResults.IsValid)
  {
    var errors = validatorResults.Errors.Select(e => new { errors = e.ErrorMessage });
    return Results.BadRequest(errors);
  }

  sneakerService.AddSneakerAsync(sneaker);
  return Results.Created("", sneaker);
});

app.MapPost("/authenticate", (IAuthenticationService authenticationService, IOptions<AuthenticationSettings> authSettings, AuthenticationRequestBody authBody) =>
{
  var user = authenticationService.ValidateUser(authBody.username, authBody.password);
  if (user == null)
  {
    return Results.Unauthorized();
  }


  // TOKEN AANMAKEN

  // symmetric key maken
  var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(authSettings.Value.SecretKey));

  // credentials om token te signen
  var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

  // claims aanmaken
  var claims = new List<Claim>
  {
    new Claim("sub", "1"),
    new Claim("username", authBody.username),
    new Claim("password", authBody.password)
  };

  var jwtToken = new JwtSecurityToken(
    authSettings.Value.Issuer,
    authSettings.Value.Audience,
    claims,
    DateTime.UtcNow,
    DateTime.Now.AddHours(1),
    signingCredentials
  );
  var tokenToReturn = new JwtSecurityTokenHandler().WriteToken(jwtToken);

  return Results.Ok(new { token = tokenToReturn });
});


app.Run();
//Hack om testen te doen werken 
public partial class Program { }
