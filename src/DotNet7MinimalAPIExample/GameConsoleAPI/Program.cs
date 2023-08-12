using GameConsoleAPI;
using GameConsoleAPI.TokenGenerators;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOptions();
builder.Services.AddOptions<JwtOptions>()
    .Bind(builder.Configuration.GetSection(nameof(JwtOptions)));

//builder.Services.AddAsymmetricJwtAuthentication(builder.Configuration);
builder.Services.AddSymmetricJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var users = new List<User>
{
    new User("demo", "demo", "Demo", "User")
};

var gameConsoles = new List<GameConsole>
{
    new GameConsole("NES", "Nintendo", 1983),
    new GameConsole("SNES", "Nintendo", 1990),
    new GameConsole("Nintento 64", "Nintendo", 1996),
    new GameConsole("Gamecube", "Nintendo", 2001),
    new GameConsole("XBox", "Microsoft", 2001),
    new GameConsole("PlayStation 1", "Sony", 1994),
    new GameConsole("PlayStation 2", "Sony", 2000),
    new GameConsole("PlayStation 3", "Sony", 2006),
    new GameConsole("PlayStation 4", "Sony", 2013),
    new GameConsole("PlayStation 5", "Sony", 2020)
};

app.MapGet("/api/game-consoles", () => Results.Ok(gameConsoles))
    .WithName("ListGameConsoles")
    .Produces<IEnumerable<GameConsole>>(200);

app.MapPost("/users/authenticate", (ITokenGeneratorService tokenGeneratorService, 
    [FromBody] AuthenticateRequestDto auth) =>
{
    if (auth == null)
    {
        return Results.BadRequest("Invalid request");
    }

    var user = users.SingleOrDefault(x => 
        x.UserName.Equals(auth.UserName) && 
        x.Password.Equals(auth.Password));

    if (user == null)
    {
        return Results.BadRequest("Invalid login");
    }
    var authToken = tokenGeneratorService.GenerateToken(user.UserName, user.Name);

    return Results.Ok(new AuthenticateSuccessResponsedto(authToken));
})
    .WithName("AuthenticateUser")
    .Produces<AuthenticateSuccessResponsedto>(200)
    .Produces(400);

// This should be private
app.MapPost("/api/game-consoles", ([FromBody] GameConsole gameConsole) =>
{
    gameConsoles.Add(gameConsole);
    return Results.Created("/api/game-consoles", gameConsole);
})
    .RequireAuthorization()
    .WithName("AddGameConsole")
    .Produces<GameConsole>(201)
    .Produces(400);

app.UseHttpsRedirection();
//app.UseAuthentication();

app.Run();
