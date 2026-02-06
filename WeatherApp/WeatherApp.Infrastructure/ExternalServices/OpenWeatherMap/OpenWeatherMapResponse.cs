using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WeatherApp.Infrastructure.ExternalServices.OpenWeatherMap
{
    internal class OpenWeatherMapResponse
    {
        [JsonPropertyName("main")]
        public MainData Main { get; set; } = new();

        [JsonPropertyName("wind")]
        public WindData Wind { get; set; } = new();

        [JsonPropertyName("weather")]
        public List<WeatherInfo> Weather { get; set; } = new();
    }

    internal class MainData
    {
        [JsonPropertyName("temp")]
        public double Temperature { get; set; }
    }

    internal class WindData
    {
        [JsonPropertyName("speed")]
        public double Speed { get; set; }
    }

    internal class WeatherInfo
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("main")]
        public string Main { get; set; } = string.Empty;
    }
}
