using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Infrastructure.Configuration
{
    public class OpenWeatherMapSettings
    {
        public const string SectionName = "OpenWeatherMap";

        public string ApiKey { get; set; } = string.Empty;
        public string BaseUrl { get; set; } = "https://api.openweathermap.org/data/2.5/weather";
        public int TimeoutSeconds { get; set; } = 30;
    }
}
