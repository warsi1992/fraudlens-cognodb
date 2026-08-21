using FraudLens.Api.Configuration;
using FraudLens.Api.Contracts;
using FraudLens.Api.Repositories;
using FraudLens.Api.Services;
using Microsoft.Extensions.Options;
using Neo4j.Driver;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.Sources.Clear();

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
    .AddJsonFile(
        $"appsettings.{builder.Environment.EnvironmentName}.json",
        optional: true,
        reloadOnChange: false)
    .AddEnvironmentVariables();

// Add services to the container.
builder.Services.Configure<Neo4jSettings>(
    builder.Configuration.GetSection("Neo4j"));
builder.Services.AddSingleton<IDriver>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<Neo4jSettings>>().Value;

    return GraphDatabase.Driver(
        settings.Uri,
        AuthTokens.Basic(settings.Username, settings.Password));
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("Angular", policy =>
    {
        policy
            .WithOrigins("https://fraudlensc.netlify.app")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ISeedService, SeedService>();
builder.Services.AddScoped<IFraudService, FraudService>();
builder.Services.AddScoped<IFraudRepository, FraudRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapSwagger();
    app.MapSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors("Angular");
app.MapControllers();

app.Run();
