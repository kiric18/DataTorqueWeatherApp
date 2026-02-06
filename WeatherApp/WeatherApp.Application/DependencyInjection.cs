using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Application.Interfaces;
using WeatherApp.Application.Services;
using WeatherApp.Application.Strategies;

namespace WeatherApp.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Register recommendation strategies (Open/Closed Principle - new strategies can be added without modifying existing code)
            services.AddSingleton<IRecommendationStrategy, SwimmingWeatherStrategy>();
            services.AddSingleton<IRecommendationStrategy, ColdWetWeatherStrategy>();
            services.AddSingleton<IRecommendationStrategy, RainyWeatherStrategy>();
            services.AddSingleton<IRecommendationStrategy, SunnyWeatherStrategy>();
            services.AddSingleton<IRecommendationStrategy, DefaultWeatherStrategy>();

            // Register services
            services.AddSingleton<IRecommendationService, RecommendationService>();
            services.AddScoped<IWeatherService, WeatherService>();
            services.AddSingleton<IRequestCounter, RequestCounter>();

            return services;
        }
    }
}
