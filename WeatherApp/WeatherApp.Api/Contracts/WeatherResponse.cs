namespace WeatherApp.Api.Contracts
{
    public record WeatherResponse(
        double TemperatureCelsius,
        double WindSpeedKph,
        string Condition,
        string Recommendation
    );
}
