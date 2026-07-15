// <copyright file="ExceptionHandlingMiddleware.cs" company="TBRZCom">
// Copyright (c) TBRZCom. All rights reserved.
// </copyright>

using System.Collections;
using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using PersonalBlog.Exceptions;
using PersonalBlog.Models;

namespace PersonalBlog.Middleware;

/// <summary>
/// Middleware that catches unhandled exceptions at the application boundary,
/// logs them exactly once with structured logging including all diagnostic
/// context from <see cref="Exception.Data"/>, and returns an appropriate
/// HTTP error response.
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExceptionHandlingMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next middleware in the pipeline.</param>
    /// <param name="logger">The logger instance.</param>
    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        this._next = next;
        this._logger = logger;
    }

    /// <summary>
    /// Invokes the middleware, catching any unhandled exception.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await this._next(context);
        }
        catch (Exception ex)
        {
            // Enrich with request-level context available at the boundary
            ex.AddData("RequestPath", context.Request.Path);
            ex.AddData("RequestMethod", context.Request.Method);
            ex.AddData("RequestScheme", context.Request.Scheme);

            if (context.User?.Identity?.IsAuthenticated == true)
            {
                ex.AddData("User", context.User.Identity.Name);
            }

            // Log each diagnostic data entry as a structured property
            foreach (DictionaryEntry entry in ex.Data)
            {
                if (entry.Key is string key)
                {
                    this._logger.LogInformation(
                        "ExceptionData: {Key} = {Value}",
                        key,
                        entry.Value);
                }
            }

            // Log the exception exactly once at the boundary
            this._logger.LogError(ex, "Unhandled exception processing {RequestMethod} {RequestPath}", context.Request.Method, context.Request.Path);

            // Produce the error response
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            // Avoid redirect loops — if the request expects JSON or is an API call,
            // return plain text; otherwise redirect to the error page.
            if (context.Request.Headers.Accept.ToString().Contains("application/json", StringComparison.OrdinalIgnoreCase))
            {
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(new
                {
                    error = "An error occurred while processing your request.",
                    requestId = Activity.Current?.Id ?? context.TraceIdentifier,
                });
            }
            else
            {
                context.Response.Redirect("/Home/Error");
            }
        }
    }
}
