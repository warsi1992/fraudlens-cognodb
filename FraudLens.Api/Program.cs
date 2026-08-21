using FraudLens.Api.Configuration;
using FraudLens.Api.Contracts;
using FraudLens.Api.Repositories;
using FraudLens.Api.Services;
using Microsoft.Extensions.Options;
using Neo4j.Driver;

var builder = WebApplication.CreateBuilder(args);

// ------------------------------------------------------------
// Configuration
// Disable configuration file watching for Render containers.
// ------------------------------------------------------------

builder.Configuration.Sources.Clear();

builder.Configuration
    .AddJsonFile(
        "appsettings.json",
        optional: false,
        reloadOnChange: false)
    .AddJsonFile(
        $"appsettings.{builder.Environment.EnvironmentName}.json",
        optional: true,
        reloadOnChange: false)
    .AddEnvironmentVariables();

// ------------------------------------------------------------
// Neo4j
// ------------------------------------------------------------

builder.Services.Configure<Neo4jSettings>(
    builder.Configuration.GetSection("Neo4j"));

builder.Services.AddSingleton<IDriver>(sp =>
{
    var settings = sp
        .GetRequiredService<IOptions<Neo4jSettings>>()
        .Value;

    return GraphDatabase.Driver(
        settings.Uri,
        AuthTokens.Basic(
            settings.Username,
            settings.Password));
});

// ------------------------------------------------------------
// CORS
// ------------------------------------------------------------

builder.Services.AddCors(options =>
{
    options.AddPolicy("Angular", policy =>
    {
        policy
            .WithOrigins(
                "https://fraudlensc.netlify.app",
                "http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// ------------------------------------------------------------
// Services
// ------------------------------------------------------------

builder.Services.AddControllers();

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ISeedService, SeedService>();
builder.Services.AddScoped<IFraudService, FraudService>();
builder.Services.AddScoped<IFraudRepository, FraudRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();

// ------------------------------------------------------------
// Application
// ------------------------------------------------------------

var app = builder.Build();

// ------------------------------------------------------------
// Swagger
// Keep enabled temporarily so we can verify the deployed API.
// ------------------------------------------------------------

app.MapOpenApi();
app.MapSwagger();
app.MapSwaggerUI();

// ------------------------------------------------------------
// HTTP pipeline
// ------------------------------------------------------------

// Do NOT use UseHttpsRedirection() here.
// Render terminates HTTPS at its proxy.

app.UseCors("Angular");

app.UseAuthorization();

app.MapControllers();

app.Run();
