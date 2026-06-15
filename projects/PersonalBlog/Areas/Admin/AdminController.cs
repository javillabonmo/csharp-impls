// <copyright file="AdminController.cs" company="TBRZCom">
// Copyright (c) TBRZCom. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalBlog.Models;
using PersonalBlog.Models.DTOs;
using PersonalBlog.Services;

namespace PersonalBlog.Areas.Admin;

/// <summary>
/// Controller for the admin area.
/// </summary>
[Area("Admin")]
[Authorize(Roles = nameof(UserTypeOptions.Admin))]
public class AdminController : Controller
{
    private readonly IArticleService _articleService;

    /// <summary>
    /// Initializes a new instance of the <see cref="AdminController"/> class.
    /// </summary>
    /// <param name="articleService">The article service.</param>
    public AdminController(IArticleService articleService)
    {
        this._articleService = articleService;
    }

    /// <summary>
    /// Displays the admin dashboard with a list of articles.
    /// </summary>
    /// <returns>The admin index view.</returns>
    [Route("/admin")]
    public async Task<IActionResult> Index()
    {
        var articles = await this._articleService.GetArticles();
        return View(articles);
    }
}
