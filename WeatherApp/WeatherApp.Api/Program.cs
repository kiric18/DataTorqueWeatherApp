using WeatherApp.Api.Middleware;
using WeatherApp.Application;
using WeatherApp.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add layers following Clean Architecture
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Add API services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Weather API",
        Version = "v1",
        Description = "A weather API that provides forecasts and clothing recommendations for Wellington commuters."
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Weather API v1");
    });
}

app.UseHttpsRedirection();

// Simulated failure middleware (every 5th request)
app.UseSimulatedFailure(failureInterval: 5);

app.UseAuthorization();
app.MapControllers();

app.Run();

// Make Program class accessible for integration tests
public partial class Program { }