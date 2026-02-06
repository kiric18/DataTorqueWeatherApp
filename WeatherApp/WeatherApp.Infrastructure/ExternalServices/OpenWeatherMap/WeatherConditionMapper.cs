using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Domain.Enums;
using WeatherApp.Domain.ValueObjects;

namespace WeatherApp.Infrastructure.ExternalServices.OpenWeatherMap
{
    internal static class WeatherConditionMapper
    {
        private const double WindyThresholdKph = 30.0;

        /// <summary>
        /// Maps OpenWeatherMap weather codes to domain weather conditions.
        /// See: https://openweathermap.org/weather-conditions
        /// </summary>
        public static WeatherCondition Map(int weatherId, WindSpeed windSpeed)
        {
            // Weather condition codes from OpenWeatherMap
            return weatherId switch
            {
                // Group 2xx: Thunderstorm
                >= 200 and < 300 => WeatherCondition.Rainy,

                // Group 3xx: Drizzle
                >= 300 and < 400 => WeatherCondition.Rainy,

                // Group 5xx: Rain
                >= 500 and < 600 => WeatherCondition.Rainy,

                // Group 6xx: Snow
                >= 600 and < 700 => WeatherCondition.Snowing,

                // Group 7xx: Atmosphere (fog, mist, etc.) - check wind
                >= 700 and < 800 => windSpeed.IsWindy(WindyThresholdKph)
                    ? WeatherCondition.Windy
                    : WeatherCondition.Sunny,

                // 800: Clear sky
                800 => windSpeed.IsWindy(WindyThresholdKph)
                    ? WeatherCondition.Windy
                    : WeatherCondition.Sunny,

                // Group 80x: Clouds
                > 800 and < 900 => windSpeed.IsWindy(WindyThresholdKph)
                    ? WeatherCondition.Windy
                    : WeatherCondition.Sunny,

                // Default
                _ => WeatherCondition.Sunny
            };
        }
    }
}
