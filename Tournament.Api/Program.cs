using Npgsql.Internal;
using PlayerDomain.ControlModule;
using PlayerDomain.ControlModule.Interfaces;
using PlayerDomain.Services;
using PlayerDomain.Services.Interfaces;
using ServerCommonModule.Database;
using ServerCommonModule.Database.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// 1. Bind environment settings
builder.Services.Configure<EnvironmentalParameters>(
    builder.Configuration.GetSection("Environment"));

// 2. Register environment abstraction
builder.Services.AddSingleton<IEnvironmentalParameters>(sp =>
{
    var options = sp.GetRequiredService<
        Microsoft.Extensions.Options.IOptions<EnvironmentalParameters>>();

    return options.Value;
});

// 3. Register DB utility parameter (required by PgUtilityFactory)
builder.Services.AddTransient<IDbUtilityParameter, DbUtilityParameter>();

// NOTE Register DB utility factory If more databases are added, this will need to be changed to a factory that can return the correct utility factory based on the environment parameters.
builder.Services.AddSingleton<IDbUtilityFactory, PgUtilityFactory>();

// 4. Register repositories
builder.Services.AddSingleton<IPlayerRepository, PlayerRepository>();

// 5. Register services
builder.Services.AddSingleton<IPlayerService, PlayerService>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
