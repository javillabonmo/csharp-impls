using Microsoft.Extensions.Options;

using WeatherAPI.Models;

namespace WeatherAPI.Services;

public interface IWeatherApiClient
{
    public  Task<WeatherResponse?> GetWeatherAsync(
        string location,
        string unitGroup = "metric",
        CancellationToken cancellationToken = default);
}
