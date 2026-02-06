using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Domain.ValueObjects;
using Xunit;

namespace WeatherApp.Domain.Tests.ValueObjects
{
    public class WindSpeedTests
    {
        [Fact]
        public void FromKph_CreatesCorrectWindSpeed()
        {
            // Act
            var wind = WindSpeed.FromKph(30.0);

            // Assert
            Assert.Equal(30.0, wind.KilometersPerHour);
        }

        [Fact]
        public void FromMetersPerSecond_ConvertsCorrectly()
        {
            // Act
            var wind = WindSpeed.FromMetersPerSecond(10.0);

            // Assert
            Assert.Equal(36.0, wind.KilometersPerHour);
        }

        [Theory]
        [InlineData(30, 30, true)]
        [InlineData(31, 30, true)]
        [InlineData(29, 30, false)]
        public void IsWindy_ReturnsCorrectResult(double kph, double threshold, bool expected)
        {
            // Arrange
            var wind = WindSpeed.FromKph(kph);

            // Act & Assert
            Assert.Equal(expected, wind.IsWindy(threshold));
        }
    }
}
