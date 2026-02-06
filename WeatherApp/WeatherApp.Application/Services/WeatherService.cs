using WeatherApp.Application.DTOs;
using WeatherApp.Application.Interfaces;
using WeatherApp.Domain.Interfaces;
using WeatherApp.Domain.ValueObjects;

namespace WeatherApp.Application.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly IWeatherProvider _weatherProvider;
        private readonly IRecommendationService _recommendationService;

        public WeatherService(IWeatherProvider weatherProvider, IRecommendationService recommendationService)
        {
            _weatherProvider = weatherProvider;
            _recommendationService = recommendationService;
        }

        public async Task<WeatherResponseDto> GetWeatherAsync(Coordinates coordinates, CancellationToken cancellationToken = default)
        {
            var weatherData = await _weatherProvider.GetWeatherAsync(coordinates, cancellationToken);
            var recommendation = _recommendationService.GetRecommendation(weatherData);

            return new WeatherResponseDto(
                TemperatureCelsius: weatherData.Temperature.Rounded(),
                WindSpeedKph: weatherData.WindSpeed.Rounded(),
                Condition: weatherData.Condition,
                Recommendation: recommendation
            );
        }
    }
}
