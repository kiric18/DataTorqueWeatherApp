using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Domain.Enums;
using WeatherApp.Domain.ValueObjects;

namespace WeatherApp.Domain.Entities
{
    public class WeatherData
    {
        public Temperature Temperature { get; }
        public WindSpeed WindSpeed { get; }
        public WeatherCondition Condition { get; }

        public WeatherData(Temperature temperature, WindSpeed windSpeed, WeatherCondition condition)
        {
            Temperature = temperature;
            WindSpeed = windSpeed;
            Condition = condition;
        }
    }
}
