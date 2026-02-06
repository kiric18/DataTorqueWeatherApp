using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Application.Interfaces;
using WeatherApp.Domain.Entities;

namespace WeatherApp.Application.Services
{
    public class RecommendationService : IRecommendationService
    {
        private readonly IEnumerable<IRecommendationStrategy> _strategies;

        public RecommendationService(IEnumerable<IRecommendationStrategy> strategies)
        {
            _strategies = strategies.OrderBy(s => s.Priority);
        }

        public string GetRecommendation(WeatherData weather)
        {
            var applicableStrategy = _strategies.FirstOrDefault(s => s.Applies(weather));
            return applicableStrategy?.GetRecommendation() ?? "Dress appropriately for the weather";
        }
    }
}
