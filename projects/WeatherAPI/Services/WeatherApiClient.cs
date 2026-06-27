using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using WeatherAPI.Models;

namespace WeatherAPI.Services;

public sealed class WeatherApiClient : IWeatherApiClient
{
    private readonly HttpClient _httpClient;
    private readonly WeatherApiOptions options;
    public WeatherApiClient(
        HttpClient httpClient,
        IOptions<WeatherApiOptions> options)
    {
        _httpClient = httpClient;
        this.options = options.Value;
    }

    public async Task<WeatherResponse?> GetWeatherAsync(
        string location,
        string unitGroup = "metric",
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(location);

        var path = $"services/timeline/{location}?unitGroup={unitGroup}&key={options.ApiKey}";

        var response = await _httpClient
                .GetFromJsonAsync<WeatherResponse>(path, cancellationToken)
                .ConfigureAwait(false);

        return response;
       }
}
