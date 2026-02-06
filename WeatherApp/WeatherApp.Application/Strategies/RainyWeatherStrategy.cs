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
    /// Recommends an umbrella when it's raining (but not cold enough for a coat).
    /// </summary>
    public class RainyWeatherStrategy : IRecommendationStrategy
    {
        public int Priority => 3;

        public bool Applies(WeatherData weather) => weather.Condition == WeatherCondition.Rainy;

        public string GetRecommendation() => "Don't forget the umbrella";
    }
}
