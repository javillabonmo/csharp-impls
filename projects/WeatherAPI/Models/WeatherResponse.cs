using System.Text.Json.Serialization;

namespace WeatherAPI.Models;

public class WeatherResponse
{
    [JsonPropertyName("address")]
    public string Address { get; set; } = string.Empty;

    [JsonPropertyName("days")]
    public List<WeatherDay> Days { get; set; } = [];

    [JsonPropertyName("currentConditions")]
    public CurrentConditions? CurrentConditions { get; set; }
}

public class WeatherDay
{
    [JsonPropertyName("datetime")]
    public string Datetime { get; set; } = string.Empty;

    [JsonPropertyName("tempmax")]
    public double TempMax { get; set; }

    [JsonPropertyName("tempmin")]
    public double TempMin { get; set; }

    [JsonPropertyName("temp")]
    public double Temp { get; set; }

    [JsonPropertyName("conditions")]
    public string Conditions { get; set; } = string.Empty;

    [JsonPropertyName("icon")]
    public string Icon { get; set; } = string.Empty;
}

public class CurrentConditions
{
    [JsonPropertyName("temp")]
    public double Temp { get; set; }

    [JsonPropertyName("feelslike")]
    public double FeelsLike { get; set; }

    [JsonPropertyName("humidity")]
    public double Humidity { get; set; }

    [JsonPropertyName("conditions")]
    public string Conditions { get; set; } = string.Empty;

    [JsonPropertyName("icon")]
    public string Icon { get; set; } = string.Empty;
}
