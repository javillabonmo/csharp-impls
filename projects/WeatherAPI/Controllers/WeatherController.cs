using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

using WeatherAPI.Models;
using WeatherAPI.Services;

namespace WeatherAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[EnableRateLimiting("fixed")]
public class WeatherController : ControllerBase
{
    private readonly IWeatherApiClient _weatherClient;
    private readonly RedisCacheService _cache;
    private readonly ILogger<WeatherController> _logger;
    private const string CacheKeyPrefix = "weather:";

    public WeatherController(
        IWeatherApiClient weatherClient,
        RedisCacheService cache,
        ILogger<WeatherController> logger)
    {
       _weatherClient = weatherClient;
       _cache = cache;
       _logger = logger;
    }


    [HttpGet]
    public async Task<IActionResult> HealthCheck([FromQuery] string location = "Bogota", [FromQuery] bool reloadCache = false)
    {
        var cacheKey = $"{CacheKeyPrefix}{location}";

        if (reloadCache)
        {
            await _cache.RemoveAsync(cacheKey);
        }

        var cached = await _cache.GetAsync<WeatherResponse>(cacheKey);

        if (cached is not null)
        {
            return Ok(new { Status = "Healthy", Source = "Cache", Result = cached });
        }

        WeatherResponse? result = null;

        try
        {
            result = await _weatherClient.GetWeatherAsync(location);
            if (result is not null)
            {
                await _cache.SetAsync(cacheKey, result, TimeSpan.FromSeconds(30));
            }

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching weather data");
            return StatusCode(500, new { Status = "Unhealthy", Error = ex.Message });
        }


        return Ok(new { Status = "Healthy", Source = "Api", Result = result });  
    }
}