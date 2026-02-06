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
    /// Default fallback recommendation when no other strategy applies.
    /// </summary>
    public class DefaultWeatherStrategy : IRecommendationStrategy
    {
        public int Priority => int.MaxValue;

        public bool Applies(WeatherData weather) => true;

        public string GetRecommendation() => "Dress appropriately for the weather";
    }
}
