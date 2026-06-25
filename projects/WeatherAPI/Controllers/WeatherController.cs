using Microsoft.AspNetCore.Mvc;
using WeatherAPI.Services;

namespace WeatherAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class WeatherController : ControllerBase
{
    private readonly IWeatherApiClient _weatherClient;

    public WeatherController(
        IWeatherApiClient weatherClient)
    {
       _weatherClient = weatherClient;
    }

    
    [HttpGet("health")]
    public async Task<IActionResult> HealthCheck()
    {
        
            var result = await _weatherClient.GetWeatherAsync("Bogota");

        return Ok(new
        {
            Status = "Healthy",
            Result = result
        });
        
        
    }
}