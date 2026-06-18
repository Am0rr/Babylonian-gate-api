using BG.Infra;
using FluentValidation.AspNetCore;
using BG.App;
using BG.Api.Middleware;
using System.Text.Json.Serialization;
using DotNetEnv;
using BG.Infra.Persistence;
using Microsoft.EntityFrameworkCore;

Env.TraversePath().Load();

var builder = WebApplication.CreateBuilder(args);

var connectionString = $"Host={Env.GetString("DB_HOST", "localhost")};" +
                       $"Port={Env.GetString("DB_PORT", "5432")};" +
                       $"Database={Env.GetString("DB_NAME", "BabylonianGateDb")};" +
                       $"Username={Env.GetString("DB_USER", "postgres")};" +
                       $"Password={Env.GetString("DB_PASSWORD")};";

builder.Configuration["ConnectionStrings:DefaultConnection"] = connectionString;

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

builder.Services.AddFluentValidationAutoValidation();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<BabylonianDbContext>();
    var logger = services.GetRequiredService<ILogger<Program>>();

    try
    {
        logger.LogInformation("Starting database migration...");
        await context.Database.MigrateAsync();
        logger.LogInformation("Database migrated successfully.");
    }
    catch (Exception ex)
    {
        logger.LogCritical(ex, "An error occurred while migrating the database.");
        throw;
    }
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
