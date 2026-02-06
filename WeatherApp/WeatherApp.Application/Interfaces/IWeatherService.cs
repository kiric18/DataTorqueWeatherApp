using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Application.DTOs;
using WeatherApp.Domain.ValueObjects;

namespace WeatherApp.Application.Interfaces
{
    public interface IWeatherService
    {
        Task<WeatherResponseDto> GetWeatherAsync(Coordinates coordinates, CancellationToken cancellationToken = default);
    }
}
