using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Domain.Enums;

namespace WeatherApp.Application.DTOs
{
    public record WeatherResponseDto(
    double TemperatureCelsius,
    double WindSpeedKph,
    WeatherCondition Condition,
    string Recommendation
);
}
