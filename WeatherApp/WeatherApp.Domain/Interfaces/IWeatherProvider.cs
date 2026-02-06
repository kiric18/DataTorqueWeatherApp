using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Domain.Entities;
using WeatherApp.Domain.ValueObjects;

namespace WeatherApp.Domain.Interfaces
{
    public interface IWeatherProvider
    {
        Task<WeatherData> GetWeatherAsync(Coordinates coordinates, CancellationToken cancellationToken = default);
    }
}
