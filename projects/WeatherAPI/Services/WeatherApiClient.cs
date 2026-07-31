using Microsoft.Extensions.Options;

using WeatherAPI.Exceptions;
using WeatherAPI.Models;

namespace WeatherAPI.Services;

public sealed class WeatherApiClient : IWeatherApiClient
{
    private readonly HttpClient _httpClient;
    private readonly WeatherApiOptions options;

    public WeatherApiClient(
        HttpClient httpClient,
        IOptions<WeatherApiOptions> options,
        ILogger<WeatherApiClient> logger)
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
        try
        {
            var response = await _httpClient
                    .GetFromJsonAsync<WeatherResponse>(path, cancellationToken)
                    .ConfigureAwait(false);

            return response;
        }
        catch (HttpRequestException ex)
        {
            
            throw ex.AddData("Location", location);
        }
        catch (TaskCanceledException ex)
        {
            throw ex.AddData("Location", location);
        }
        }
}

