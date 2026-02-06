using WeatherApp.Application.Strategies;
using WeatherApp.Domain.Entities;
using WeatherApp.Domain.Enums;
using WeatherApp.Domain.ValueObjects;
using Xunit;

namespace WeatherApp.Application.Tests.Strategies
{
    public class RecommendationStrategyTests
    {
        [Theory]
        [InlineData(26)]
        [InlineData(30)]
        [InlineData(40)]
        public void SwimmingWeatherStrategy_Applies_WhenOver25Degrees(double celsius)
        {
            // Arrange
            var strategy = new SwimmingWeatherStrategy();
            var weather = CreateWeather(celsius, WeatherCondition.Sunny);

            // Act & Assert
            Assert.True(strategy.Applies(weather));
            Assert.Equal("It's a great day for a swim", strategy.GetRecommendation());
        }

        [Theory]
        [InlineData(25)]
        [InlineData(20)]
        public void SwimmingWeatherStrategy_DoesNotApply_When25OrBelow(double celsius)
        {
            // Arrange
            var strategy = new SwimmingWeatherStrategy();
            var weather = CreateWeather(celsius, WeatherCondition.Sunny);

            // Act & Assert
            Assert.False(strategy.Applies(weather));
        }

        [Theory]
        [InlineData(10, WeatherCondition.Rainy)]
        [InlineData(14.9, WeatherCondition.Rainy)]
        [InlineData(5, WeatherCondition.Snowing)]
        [InlineData(-5, WeatherCondition.Snowing)]
        public void ColdWetWeatherStrategy_Applies_WhenColdAndWet(double celsius, WeatherCondition condition)
        {
            // Arrange
            var strategy = new ColdWetWeatherStrategy();
            var weather = CreateWeather(celsius, condition);

            // Act & Assert
            Assert.True(strategy.Applies(weather));
            Assert.Equal("Don't forget to bring a coat", strategy.GetRecommendation());
        }

        [Theory]
        [InlineData(15, WeatherCondition.Rainy)]
        [InlineData(10, WeatherCondition.Sunny)]
        [InlineData(10, WeatherCondition.Windy)]
        public void ColdWetWeatherStrategy_DoesNotApply_WhenNotColdOrNotWet(double celsius, WeatherCondition condition)
        {
            // Arrange
            var strategy = new ColdWetWeatherStrategy();
            var weather = CreateWeather(celsius, condition);

            // Act & Assert
            Assert.False(strategy.Applies(weather));
        }

        [Theory]
        [InlineData(15)]
        [InlineData(20)]
        [InlineData(25)]
        public void RainyWeatherStrategy_Applies_WhenRaining(double celsius)
        {
            // Arrange
            var strategy = new RainyWeatherStrategy();
            var weather = CreateWeather(celsius, WeatherCondition.Rainy);

            // Act & Assert
            Assert.True(strategy.Applies(weather));
            Assert.Equal("Don't forget the umbrella", strategy.GetRecommendation());
        }

        [Fact]
        public void SunnyWeatherStrategy_Applies_WhenSunny()
        {
            // Arrange
            var strategy = new SunnyWeatherStrategy();
            var weather = CreateWeather(20, WeatherCondition.Sunny);

            // Act & Assert
            Assert.True(strategy.Applies(weather));
            Assert.Equal("Don't forget to bring a hat", strategy.GetRecommendation());
        }

        [Fact]
        public void DefaultWeatherStrategy_AlwaysApplies()
        {
            // Arrange
            var strategy = new DefaultWeatherStrategy();
            var weather = CreateWeather(20, WeatherCondition.Windy);

            // Act & Assert
            Assert.True(strategy.Applies(weather));
            Assert.Equal("Dress appropriately for the weather", strategy.GetRecommendation());
        }

        [Fact]
        public void Strategies_HaveCorrectPriorityOrder()
        {
            // Arrange
            var strategies = new List<object>
        {
            new SwimmingWeatherStrategy(),
            new ColdWetWeatherStrategy(),
            new RainyWeatherStrategy(),
            new SunnyWeatherStrategy(),
            new DefaultWeatherStrategy()
        };

            // Act
            var ordered = strategies
                .Cast<dynamic>()
                .OrderBy(s => (int)s.Priority)
                .ToList();

            // Assert
            Assert.IsType<SwimmingWeatherStrategy>(ordered[0]);
            Assert.IsType<ColdWetWeatherStrategy>(ordered[1]);
            Assert.IsType<RainyWeatherStrategy>(ordered[2]);
            Assert.IsType<SunnyWeatherStrategy>(ordered[3]);
            Assert.IsType<DefaultWeatherStrategy>(ordered[4]);
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
