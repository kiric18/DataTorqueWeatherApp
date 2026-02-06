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
    /// Recommends a hat on sunny days.
    /// </summary>
    public class SunnyWeatherStrategy : IRecommendationStrategy
    {
        public int Priority => 4;

        public bool Applies(WeatherData weather) => weather.Condition == WeatherCondition.Sunny;

        public string GetRecommendation() => "Don't forget to bring a hat";
    }
}
