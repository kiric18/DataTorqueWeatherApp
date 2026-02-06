using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Application.Interfaces;
using WeatherApp.Domain.Entities;
using WeatherApp.Domain.Enums;

namespace WeatherApp.Application.Strategies
{
    /// <summary>
    /// Recommends a coat when it's cold (below 15°C) and raining or snowing.
    /// </summary>
    public class ColdWetWeatherStrategy : IRecommendationStrategy
    {
        private const double ColdTemperatureThreshold = 15.0;

        public int Priority => 2;

        public bool Applies(WeatherData weather) =>
            weather.Temperature.IsBelow(ColdTemperatureThreshold) &&
            (weather.Condition == WeatherCondition.Rainy || weather.Condition == WeatherCondition.Snowing);

        public string GetRecommendation() => "Don't forget to bring a coat";
    }
}
