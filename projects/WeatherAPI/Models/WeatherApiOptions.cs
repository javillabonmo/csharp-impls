namespace WeatherAPI.Models;

public sealed class WeatherApiOptions
{
    
    public const string SectionName = "WeatherApi";

    public string ApiKey { get; set; } = string.Empty;

    public string BaseUrl { get; set; } = string.Empty;
}
