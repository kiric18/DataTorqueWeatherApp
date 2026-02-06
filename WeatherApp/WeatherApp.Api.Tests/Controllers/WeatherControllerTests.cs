using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Api.Contracts;
using WeatherApp.Api.Controllers;
using WeatherApp.Application.DTOs;
using WeatherApp.Application.Interfaces;
using WeatherApp.Domain.Enums;
using WeatherApp.Domain.ValueObjects;
using Xunit;

namespace WeatherApp.Api.Tests.Controllers
{
    public class WeatherControllerTests
    {
        private readonly Mock<IWeatherService> _mockWeatherService;
        private readonly Mock<ILogger<WeatherController>> _mockLogger;
        private readonly WeatherController _controller;

        public WeatherControllerTests()
        {
            _mockWeatherService = new Mock<IWeatherService>();
            _mockLogger = new Mock<ILogger<WeatherController>>();
            _controller = new WeatherController(_mockWeatherService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetWeather_ValidRequest_ReturnsOk()
        {
            // Arrange
            var dto = new WeatherResponseDto(18.5, 12.3, WeatherCondition.Sunny, "Don't forget to bring a hat");
            _mockWeatherService
                .Setup(s => s.GetWeatherAsync(It.IsAny<Coordinates>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dto);

            // Act
            var result = await _controller.GetWeather(-41.2865, 174.7762, CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<WeatherResponse>(okResult.Value);
            Assert.Equal(18.5, response.TemperatureCelsius);
            Assert.Equal("Sunny", response.Condition);
        }

        [Theory]
        [InlineData(-91, 0)]
        [InlineData(91, 0)]
        public async Task GetWeather_InvalidLatitude_ReturnsBadRequest(double lat, double lon)
        {
            // Act
            var result = await _controller.GetWeather(lat, lon, CancellationToken.None);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var error = Assert.IsType<ErrorResponse>(badRequest.Value);
            Assert.Contains("Latitude", error.Error);
        }

        [Theory]
        [InlineData(0, -181)]
        [InlineData(0, 181)]
        public async Task GetWeather_InvalidLongitude_ReturnsBadRequest(double lat, double lon)
        {
            // Act
            var result = await _controller.GetWeather(lat, lon, CancellationToken.None);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var error = Assert.IsType<ErrorResponse>(badRequest.Value);
            Assert.Contains("Longitude", error.Error);
        }

        [Fact]
        public async Task GetWeather_ServiceThrowsHttpException_Returns503()
        {
            // Arrange
            _mockWeatherService
                .Setup(s => s.GetWeatherAsync(It.IsAny<Coordinates>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new HttpRequestException("Connection failed"));

            // Act
            var result = await _controller.GetWeather(0, 0, CancellationToken.None);

            // Assert
            var statusResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(503, statusResult.StatusCode);
        }
    }
}
