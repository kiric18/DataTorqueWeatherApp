using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeatherApp.Domain.Interfaces;
using WeatherApp.Infrastructure.Configuration;
using WeatherApp.Infrastructure.ExternalServices.OpenWeatherMap;

namespace WeatherApp.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Configure settings using the Bind approach
            var settings = new OpenWeatherMapSettings();
            configuration.GetSection(OpenWeatherMapSettings.SectionName).Bind(settings);

            services.Configure<OpenWeatherMapSettings>(
                options =>
                {
                    options.ApiKey = settings.ApiKey;
                    options.BaseUrl = settings.BaseUrl;
                    options.TimeoutSeconds = settings.TimeoutSeconds;
                });

            // Register HttpClient factory
            services.AddHttpClient();

            // Register the weather provider with a configured HttpClient
            services.AddScoped<IWeatherProvider>(sp =>
            {
                var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
                var client = httpClientFactory.CreateClient();
                client.Timeout = TimeSpan.FromSeconds(settings.TimeoutSeconds);
                client.DefaultRequestHeaders.Add("Accept", "application/json");

                var options = Microsoft.Extensions.Options.Options.Create(settings);
                var logger = sp.GetRequiredService<Microsoft.Extensions.Logging.ILogger<OpenWeatherMapProvider>>();

                return new OpenWeatherMapProvider(client, options, logger);
            });

            return services;
        }
    }
}
