using Microsoft.AspNetCore.Mvc;
using WeatherApp.Api.Contracts;
using WeatherApp.Application.Interfaces;
using WeatherApp.Domain.ValueObjects;

namespace WeatherApp.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _weatherService;
        private readonly ILogger<WeatherController> _logger;

        public WeatherController(IWeatherService weatherService, ILogger<WeatherController> logger)
        {
            _weatherService = weatherService;
            _logger = logger;
        }

        /// <summary>
        /// Gets current weather and clothing recommendation for the specified coordinates.
        /// </summary>
        /// <param name="latitude">Latitude (-90 to 90)</param>
        /// <param name="longitude">Longitude (-180 to 180)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Weather data with recommendation</returns>
        [HttpGet]
        [ProducesResponseType(typeof(WeatherResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> GetWeather(
            [FromQuery] double latitude,
            [FromQuery] double longitude,
            CancellationToken cancellationToken)
        {
            // Validate and create coordinates value object
            var coordinatesResult = Coordinates.Create(latitude, longitude);

            if (!coordinatesResult.IsSuccess)
            {
                return BadRequest(new ErrorResponse(coordinatesResult.Error!));
            }

            try
            {
                var weather = await _weatherService.GetWeatherAsync(coordinatesResult.Value, cancellationToken);

                var response = new WeatherResponse(
                    TemperatureCelsius: weather.TemperatureCelsius,
                    WindSpeedKph: weather.WindSpeedKph,
                    Condition: weather.Condition.ToString(),
                    Recommendation: weather.Recommendation
                );

                return Ok(response);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Failed to fetch weather data from upstream service");
                return StatusCode(StatusCodes.Status503ServiceUnavailable,
                    new ErrorResponse("Failed to fetch weather data. Please try again later."));
            }
        }
    }
}
