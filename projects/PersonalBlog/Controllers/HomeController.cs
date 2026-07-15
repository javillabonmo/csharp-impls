// <copyright file="HomeController.cs" company="TBRZCom">
// Copyright (c) TBRZCom. All rights reserved.
// </copyright>

using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PersonalBlog.Models;
using PersonalBlog.Models.DTOs;
using PersonalBlog.Services;

namespace PersonalBlog.Controllers;

/// <summary>
/// Controller for the home page and error handling.
/// </summary>
public class HomeController : Controller
{
    private readonly IArticleService _articleService;
    private readonly ILogger<HomeController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="HomeController"/> class.
    /// </summary>
    /// <param name="articleService">The article service.</param>
    /// <param name="logger">The logger instance.</param>
    public HomeController(IArticleService articleService, ILogger<HomeController> logger)
    {
        this._articleService = articleService;
        this._logger = logger;
    }

    /// <summary>
    /// Redirects the root URL to the home page.
    /// </summary>
    /// <returns>A redirect to the Index action.</returns>
    [HttpGet]
    [Route("/")]
    public async Task<IActionResult> Root()
    {
        return RedirectToAction("Index");
    }

    /// <summary>
    /// Displays the home page with a list of articles.
    /// </summary>
    /// <returns>The home page view.</returns>
    [HttpGet]
    [Route("/home")]
    public async Task<IActionResult> Index()
    {
        var articles = await this._articleService.GetArticles();
        return View(articles);
    }

    /// <summary>
    /// Displays the error page.
    /// </summary>
    /// <returns>The error view.</returns>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

        // If reached via UseExceptionHandler fallback, log any stored exception
        var exceptionFeature = HttpContext.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerPathFeature>();
        if (exceptionFeature?.Error is { } ex)
        {
            this._logger.LogError(ex, "Fallback exception handler for request {RequestId}", requestId);
        }

        return View(new ErrorViewModel { RequestId = requestId });
    }
}
