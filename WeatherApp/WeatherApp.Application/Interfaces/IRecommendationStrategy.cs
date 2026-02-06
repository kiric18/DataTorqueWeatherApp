using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Domain.Entities;

namespace WeatherApp.Application.Interfaces
{
    public interface IRecommendationStrategy
    {
        int Priority { get; }
        bool Applies(WeatherData weather);
        string GetRecommendation();
    }
}
