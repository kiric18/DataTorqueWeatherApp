using WeatherApp.Api.Middleware;
using WeatherApp.Application;
using WeatherApp.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add layers following Clean Architecture
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Add CORS for React client - MUST be before AddControllers
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:3000")
          .AllowAnyHeader()
          .AllowAnyMethod();
    });
});

// Add API services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Weather API",
        Version = "v1",
        Description = "A weather API that provides forecasts and clothing recommendations."
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// REMOVE or comment out HTTPS redirection for local development
// app.UseHttpsRedirection();

// CORS must be called BEFORE other middleware
app.UseCors();

// Simulated failure middleware (every 5th request)
app.UseSimulatedFailure(failureInterval: 5);

app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program { }