using Serilog.Context;

namespace ExpenseTracker.Observability;

/// <summary>
/// Pushes per-request properties into the Serilog LogContext so every log event
/// within the request automatically carries correlation identifiers.
/// </summary>
public class RequestContextMiddleware
{
    private readonly RequestDelegate _next;

    public RequestContextMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        using (LogContext.PushProperty("RequestId", context.TraceIdentifier))
        using (LogContext.PushProperty("RequestPath", context.Request.Path))
        using (LogContext.PushProperty("RequestMethod", context.Request.Method))
        {
            await _next(context);
        }
    }
}
