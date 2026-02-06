using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Domain.ValueObjects;
using Xunit;

namespace WeatherApp.Domain.Tests.ValueObjects
{
    public class TemperatureTests
    {
        [Fact]
        public void FromCelsius_CreatesCorrectTemperature()
        {
            // Act
            var temp = Temperature.FromCelsius(25.5);

            // Assert
            Assert.Equal(25.5, temp.Celsius);
        }

        [Fact]
        public void FromKelvin_ConvertsCorrectly()
        {
            // Act
            var temp = Temperature.FromKelvin(298.15);

            // Assert
            Assert.Equal(25.0, temp.Celsius, precision: 1);
        }

        [Theory]
        [InlineData(26, 25, true)]
        [InlineData(25, 25, false)]
        [InlineData(24, 25, false)]
        public void IsAbove_ReturnsCorrectResult(double celsius, double threshold, bool expected)
        {
            // Arrange
            var temp = Temperature.FromCelsius(celsius);

            // Act & Assert
            Assert.Equal(expected, temp.IsAbove(threshold));
        }

        [Theory]
        [InlineData(14, 15, true)]
        [InlineData(15, 15, false)]
        [InlineData(16, 15, false)]
        public void IsBelow_ReturnsCorrectResult(double celsius, double threshold, bool expected)
        {
            // Arrange
            var temp = Temperature.FromCelsius(celsius);

            // Act & Assert
            Assert.Equal(expected, temp.IsBelow(threshold));
        }
    }
}
