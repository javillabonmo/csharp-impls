using WeatherAPI.Models;

namespace WeatherAPI.Services;

public interface IWeatherApiClient
{
    Task<WeatherResponse?> GetWeatherAsync(
        string location,
        string unitGroup = "metric",
        CancellationToken cancellationToken = default);
}
