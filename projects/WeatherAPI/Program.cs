using Microsoft.Extensions.Options;
using StackExchange.Redis;
using WeatherAPI.Models;
using WeatherAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = ConfigurationOptions.Parse(
        builder.Configuration.GetConnectionString("Redis")!);

    configuration.AbortOnConnectFail = false;
    configuration.ConnectRetry = 3;
    configuration.ConnectTimeout = 5000;

    return ConnectionMultiplexer.Connect(configuration);
});


builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.Configure<WeatherApiOptions>(
    builder.Configuration.GetSection(WeatherApiOptions.SectionName));




builder.Services
    .AddHttpClient<IWeatherApiClient, WeatherApiClient>((serviceProvider, client) =>
    {
        var settings = serviceProvider
            .GetRequiredService<IOptions<WeatherApiOptions>>()
            .Value;

        client.BaseAddress = new Uri(settings.BaseUrl);

        client.Timeout = TimeSpan.FromSeconds(30);
    })
    .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
    {
        PooledConnectionLifetime = TimeSpan.FromMinutes(15),
    })
    .SetHandlerLifetime(Timeout.InfiniteTimeSpan);


builder.Services.AddScoped<RedisCacheService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthorization();
app.MapControllers();
app.Run();
