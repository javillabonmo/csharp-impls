using System.Collections;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        LogException(exception);

        var problemDetails = CreateProblemDetails(context, exception);

        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = (int)problemDetails.Status!;

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails, jsonOptions));
    }

    private void LogException(Exception exception)
    {
        foreach (DictionaryEntry entry in exception.Data)
        {
            _logger.LogInformation(
                "Exception context — {Key}: {Value}",
                entry.Key,
                entry.Value);
        }

        _logger.LogError(exception, "Unhandled exception");
    }

    private static ProblemDetails CreateProblemDetails(HttpContext context, Exception exception)
    {
        var (statusCode, title) = exception switch
        {
            KeyNotFoundException => (HttpStatusCode.NotFound, "Resource not found"),
            ArgumentException => (HttpStatusCode.BadRequest, "Invalid request"),
            UnauthorizedAccessException => (HttpStatusCode.Forbidden, "Access denied"),
            OperationCanceledException => (HttpStatusCode.ServiceUnavailable, "Operation was cancelled"),
            TimeoutException => (HttpStatusCode.GatewayTimeout, "Operation timed out"),
            Microsoft.IdentityModel.Tokens.SecurityTokenException =>
                (HttpStatusCode.Unauthorized, "Authentication failed"),
            _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred")
        };

        return new ProblemDetails
        {
            Status = (int)statusCode,
            Title = title,
            Type = $"https://httpstatuses.com/{(int)statusCode}",
            Instance = context.Request.Path,
            Extensions =
            {
                ["traceId"] = context.TraceIdentifier
            }
        };
    }
}
