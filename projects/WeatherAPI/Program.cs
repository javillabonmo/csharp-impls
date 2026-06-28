using System.Threading.RateLimiting;

using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;

using Scalar.AspNetCore;

using StackExchange.Redis;

using WeatherAPI.Models;
using WeatherAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(builder.Configuration.GetSection("AllowedOrigins").Get<string[]>()).AllowAnyHeader().AllowAnyMethod();
    });
});


builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = ConfigurationOptions.Parse(
        builder.Configuration.GetConnectionString("Redis")!);

    configuration.AbortOnConnectFail = false;
    configuration.ConnectRetry = 3;
    configuration.ConnectTimeout = 5000;

    return ConnectionMultiplexer.Connect(configuration);
});


builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed", opt =>
    {
        opt.PermitLimit = 100;
        opt.Window = TimeSpan.FromMinutes(1);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 0;
    });
});


builder.Services.AddControllers();
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info = new()
        {
            Title = "WeatherAPI",
            Version = "v1",
            Description = "Weather data API with Redis caching, rate limiting, and external API integration."
        };
        return Task.CompletedTask;
    });
});

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
    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("WeatherAPI")
            .WithTheme(ScalarTheme.BluePlanet)
            .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}

app.UseRateLimiter();
app.UseAuthorization();
app.MapControllers();
app.Run();
