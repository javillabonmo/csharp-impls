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
    private const string CacheKeyPrefix = "weather:";

    public WeatherController(
        IWeatherApiClient weatherClient,
        RedisCacheService cache)
    {
       _weatherClient = weatherClient;
       _cache = cache;
    }


    [HttpGet]
    public async Task<IActionResult> HealthCheck([FromQuery] string location = "Bogota", [FromQuery] bool reloadCache = false)
    {
        // En este caso lo que se desea cachear es la "locacion",
        // sea por " addresses, partial addresses or latitude,longitude values"

        // que se esta haciendo? antes de hacer la llamada a la API externa
        // se verifica si el resultado ya está en cache "cache-hit"
        // Si está en cache, se devuelve el resultado almacenado.
        // Si no está en caché, se realiza la llamada a la API externa, se almacena el resultado en caché y luego se devuelve.

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

        var result = await _weatherClient.GetWeatherAsync(location);
        if (result is not null)
        {
            await _cache.SetAsync(cacheKey, result, TimeSpan.FromSeconds(30));
        }

        return Ok(new { Status = "Healthy", Source = "Api", Result = result });  
    }
}