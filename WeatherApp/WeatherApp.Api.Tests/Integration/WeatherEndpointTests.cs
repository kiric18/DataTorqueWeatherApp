using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Api.Contracts;
using WeatherApp.Application.DTOs;
using WeatherApp.Application.Interfaces;
using WeatherApp.Application.Services;
using WeatherApp.Domain.Enums;
using WeatherApp.Domain.Interfaces;
using WeatherApp.Domain.ValueObjects;
using Xunit;

namespace WeatherApp.Api.Tests.Integration
{
    public class WeatherEndpointTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public WeatherEndpointTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        private HttpClient CreateClientWithMocks(WeatherResponseDto? responseDto = null)
        {
            var mockWeatherService = new Mock<IWeatherService>();

            if (responseDto != null)
            {
                mockWeatherService
                    .Setup(s => s.GetWeatherAsync(It.IsAny<Coordinates>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(responseDto);
            }

            return _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Remove existing services
                    RemoveService<IWeatherService>(services);
                    RemoveService<IWeatherProvider>(services);
                    RemoveService<IRequestCounter>(services);

                    // Add mocks
                    services.AddSingleton(mockWeatherService.Object);
                    services.AddSingleton<IRequestCounter>(new RequestCounter());
                });
            }).CreateClient();
        }

        private static void RemoveService<T>(IServiceCollection services)
        {
            var descriptor = services.FirstOrDefault(d => d.ServiceType == typeof(T));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }
        }

        [Fact]
        public async Task GetWeather_ReturnsCorrectResponse()
        {
            // Arrange
            var dto = new WeatherResponseDto(18.5, 12.3, WeatherCondition.Sunny, "Don't forget to bring a hat");
            var client = CreateClientWithMocks(dto);

            // Act
            var response = await client.GetAsync("/weather?latitude=-41.2865&longitude=174.7762");

            // Assert
            response.EnsureSuccessStatusCode();
            var weather = await response.Content.ReadFromJsonAsync<WeatherResponse>();
            Assert.NotNull(weather);
            Assert.Equal(18.5, weather.TemperatureCelsius);
            Assert.Equal("Sunny", weather.Condition);
        }

        [Fact]
        public async Task GetWeather_InvalidCoordinates_ReturnsBadRequest()
        {
            // Arrange
            var client = CreateClientWithMocks(new WeatherResponseDto(0, 0, WeatherCondition.Sunny, ""));

            // Act
            var response = await client.GetAsync("/weather?latitude=100&longitude=0");

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetWeather_EveryFifthRequest_Returns503()
        {
            // Arrange
            var dto = new WeatherResponseDto(20, 10, WeatherCondition.Sunny, "Don't forget to bring a hat");
            var client = CreateClientWithMocks(dto);

            // Act - Make 5 requests
            var responses = new List<HttpResponseMessage>();
            for (int i = 0; i < 5; i++)
            {
                responses.Add(await client.GetAsync("/weather?latitude=0&longitude=0"));
            }

            // Assert
            Assert.Equal(HttpStatusCode.OK, responses[0].StatusCode);
            Assert.Equal(HttpStatusCode.OK, responses[1].StatusCode);
            Assert.Equal(HttpStatusCode.OK, responses[2].StatusCode);
            Assert.Equal(HttpStatusCode.OK, responses[3].StatusCode);
            Assert.Equal(HttpStatusCode.ServiceUnavailable, responses[4].StatusCode);
        }

        [Fact]
        public async Task GetWeather_MultipleOf5Requests_Returns503()
        {
            // Arrange
            var dto = new WeatherResponseDto(20, 10, WeatherCondition.Sunny, "Don't forget to bring a hat");
            var client = CreateClientWithMocks(dto);

            // Act - Make 10 requests
            var responses = new List<HttpResponseMessage>();
            for (int i = 0; i < 10; i++)
            {
                responses.Add(await client.GetAsync("/weather?latitude=0&longitude=0"));
            }

            // Assert - 5th and 10th should be 503
            Assert.Equal(HttpStatusCode.ServiceUnavailable, responses[4].StatusCode);
            Assert.Equal(HttpStatusCode.ServiceUnavailable, responses[9].StatusCode);
        }
    }
}
