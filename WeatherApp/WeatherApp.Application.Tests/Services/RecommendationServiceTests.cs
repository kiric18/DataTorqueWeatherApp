using WeatherApp.Application.Interfaces;
using WeatherApp.Application.Services;
using WeatherApp.Application.Strategies;
using WeatherApp.Domain.Entities;
using WeatherApp.Domain.Enums;
using WeatherApp.Domain.ValueObjects;
using Xunit;

namespace WeatherApp.Application.Tests.Services
{
    public class RecommendationServiceTests
    {
        private readonly IRecommendationService _service;

        public RecommendationServiceTests()
        {
            var strategies = new IRecommendationStrategy[]
            {
            new SwimmingWeatherStrategy(),
            new ColdWetWeatherStrategy(),
            new RainyWeatherStrategy(),
            new SunnyWeatherStrategy(),
            new DefaultWeatherStrategy()
            };

            _service = new RecommendationService(strategies);
        }

        [Fact]
        public void GetRecommendation_HotAndRainy_ReturnsSwimmingNotUmbrella()
        {
            // Arrange - Hot weather should take priority over rain
            var weather = CreateWeather(30, WeatherCondition.Rainy);

            // Act
            var recommendation = _service.GetRecommendation(weather);

            // Assert
            Assert.Equal("It's a great day for a swim", recommendation);
        }

        [Fact]
        public void GetRecommendation_ColdAndRainy_ReturnsCoatNotUmbrella()
        {
            // Arrange - Cold + wet should take priority over just rainy
            var weather = CreateWeather(10, WeatherCondition.Rainy);

            // Act
            var recommendation = _service.GetRecommendation(weather);

            // Assert
            Assert.Equal("Don't forget to bring a coat", recommendation);
        }

        [Fact]
        public void GetRecommendation_MildAndRainy_ReturnsUmbrella()
        {
            // Arrange - Not cold enough for coat, not hot enough for swim
            var weather = CreateWeather(18, WeatherCondition.Rainy);

            // Act
            var recommendation = _service.GetRecommendation(weather);

            // Assert
            Assert.Equal("Don't forget the umbrella", recommendation);
        }

        [Fact]
        public void GetRecommendation_Sunny_ReturnsHat()
        {
            // Arrange
            var weather = CreateWeather(22, WeatherCondition.Sunny);

            // Act
            var recommendation = _service.GetRecommendation(weather);

            // Assert
            Assert.Equal("Don't forget to bring a hat", recommendation);
        }

        [Fact]
        public void GetRecommendation_Windy_ReturnsDefault()
        {
            // Arrange
            var weather = CreateWeather(20, WeatherCondition.Windy);

            // Act
            var recommendation = _service.GetRecommendation(weather);

            // Assert
            Assert.Equal("Dress appropriately for the weather", recommendation);
        }

        private static WeatherData CreateWeather(double celsius, WeatherCondition condition)
        {
            return new WeatherData(
                Temperature.FromCelsius(celsius),
                WindSpeed.FromKph(10),
                condition
            );
        }
    }
}
