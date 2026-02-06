using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Domain.ValueObjects;
using Xunit;

namespace WeatherApp.Domain.Tests.ValueObjects
{
    public class CoordinatesTests
    {
        [Theory]
        [InlineData(0, 0)]
        [InlineData(-90, -180)]
        [InlineData(90, 180)]
        [InlineData(-41.2865, 174.7762)] // Wellington
        public void Create_ValidCoordinates_ReturnsSuccess(double latitude, double longitude)
        {
            // Act
            var result = Coordinates.Create(latitude, longitude);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(latitude, result.Value.Latitude);
            Assert.Equal(longitude, result.Value.Longitude);
        }

        [Theory]
        [InlineData(-91, 0)]
        [InlineData(91, 0)]
        public void Create_InvalidLatitude_ReturnsFailure(double latitude, double longitude)
        {
            // Act
            var result = Coordinates.Create(latitude, longitude);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Latitude", result.Error);
        }

        [Theory]
        [InlineData(0, -181)]
        [InlineData(0, 181)]
        public void Create_InvalidLongitude_ReturnsFailure(double latitude, double longitude)
        {
            // Act
            var result = Coordinates.Create(latitude, longitude);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Longitude", result.Error);
        }
    }
}
