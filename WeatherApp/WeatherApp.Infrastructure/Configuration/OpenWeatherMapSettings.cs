using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Infrastructure.Configuration
{
    public class OpenWeatherMapSettings
    {
        public const string SectionName = "OpenWeatherMap";

        [Required(ErrorMessage = "OpenWeatherMap API key is required. Set it using: dotnet user-secrets set \"OpenWeatherMap:ApiKey\" \"your-key\"")]
        public string ApiKey { get; set; } = string.Empty;
        public string BaseUrl { get; set; } = "https://api.openweathermap.org/data/2.5/weather";
        public int TimeoutSeconds { get; set; } = 30;
    }
}
