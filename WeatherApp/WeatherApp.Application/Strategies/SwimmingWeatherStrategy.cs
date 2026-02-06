using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Application.Interfaces;
using WeatherApp.Domain.Entities;

namespace WeatherApp.Application.Strategies
{
    /// <summary>
    /// Recommends swimming when temperature is above 25°C.
    /// Highest priority as hot weather takes precedence.
    /// </summary>
    public class SwimmingWeatherStrategy : IRecommendationStrategy
    {
        private const double HotTemperatureThreshold = 25.0;

        public int Priority => 1;

        public bool Applies(WeatherData weather) => weather.Temperature.IsAbove(HotTemperatureThreshold);

        public string GetRecommendation() => "It's a great day for a swim";
    }
}
