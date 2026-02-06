using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Domain.Entities;
using WeatherApp.Domain.Interfaces;
using WeatherApp.Domain.ValueObjects;
using WeatherApp.Infrastructure.Configuration;

namespace WeatherApp.Infrastructure.ExternalServices.OpenWeatherMap
{
    public class OpenWeatherMapProvider : IWeatherProvider
    {
        private readonly HttpClient _httpClient;
        private readonly OpenWeatherMapSettings _settings;
        private readonly ILogger<OpenWeatherMapProvider> _logger;

        public OpenWeatherMapProvider(
            HttpClient httpClient,
            IOptions<OpenWeatherMapSettings> settings,
            ILogger<OpenWeatherMapProvider> logger)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
            _logger = logger;
        }

        public async Task<WeatherData> GetWeatherAsync(Coordinates coordinates, CancellationToken cancellationToken = default)
        {
            var url = BuildRequestUrl(coordinates);

            _logger.LogDebug("Fetching weather data for coordinates: {Lat}, {Lon}",
                coordinates.Latitude, coordinates.Longitude);

            var response = await _httpClient.GetAsync(url, cancellationToken);
            response.EnsureSuccessStatusCode();

            var apiResponse = await response.Content.ReadFromJsonAsync<OpenWeatherMapResponse>(cancellationToken)
                ?? throw new InvalidOperationException("Failed to deserialize weather data");

            return MapToWeatherData(apiResponse);
        }

        private string BuildRequestUrl(Coordinates coordinates)
        {
            return $"{_settings.BaseUrl}?lat={coordinates.Latitude}&lon={coordinates.Longitude}&appid={_settings.ApiKey}&units=metric";
        }

        private static WeatherData MapToWeatherData(OpenWeatherMapResponse response)
        {
            var temperature = Temperature.FromCelsius(response.Main.Temperature);
            var windSpeed = WindSpeed.FromMetersPerSecond(response.Wind.Speed);

            var weatherId = response.Weather.FirstOrDefault()?.Id ?? 800;
            var condition = WeatherConditionMapper.Map(weatherId, windSpeed);

            return new WeatherData(temperature, windSpeed, condition);
        }
    }
}
