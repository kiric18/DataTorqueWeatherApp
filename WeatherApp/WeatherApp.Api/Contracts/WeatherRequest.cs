using System.ComponentModel.DataAnnotations;

namespace WeatherApp.Api.Contracts
{
    public record WeatherRequest(
        [Required] double Latitude,
        [Required] double Longitude
    );
}
