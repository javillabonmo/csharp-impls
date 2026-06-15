// <copyright file="ArticleController.cs" company="TBRZCom">
// Copyright (c) TBRZCom. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalBlog.Models.DTOs;
using PersonalBlog.Services;

namespace PersonalBlog.Controllers;

/// <summary>
/// Controller for managing articles.
/// </summary>
public class ArticleController : Controller
{
    private readonly IArticleService _articleService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ArticleController"/> class.
    /// </summary>
    /// <param name="articleService">The article service.</param>
    public ArticleController(IArticleService articleService)
    {
        this._articleService = articleService;
    }

    /// <summary>
    /// Displays the detail of an article.
    /// </summary>
    /// <param name="id">The article ID.</param>
    /// <returns>The article detail view.</returns>
    [Route("/article/{id}")]
    public async Task<IActionResult> Detail(int id)
    {
        try
        {
            var article = await this._articleService.GetArticle(id);
            return View(article);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Displays the article creation form.
    /// </summary>
    /// <returns>The creation view.</returns>
    [Authorize(Roles = "Admin")]
    [Route("[action]")]
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    /// <summary>
    /// Handles article creation submission.
    /// </summary>
    /// <param name="request">The article request data.</param>
    /// <returns>A redirect to the new article detail, or the creation view on failure.</returns>
    [Authorize(Roles = "Admin")]
    [Route("[action]")]
    [HttpPost]
    public async Task<IActionResult> Create(ArticleRequest request)
    {
        if (!ModelState.IsValid)
        {
            return View(request);
        }

        try
        {
            var article = await this._articleService.AddArticle(request);
            return RedirectToAction("Detail", new { id = article.Id });
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(request);
        }
    }

    /// <summary>
    /// Displays the article edit form.
    /// </summary>
    /// <param name="id">The article ID.</param>
    /// <returns>The edit view.</returns>
    [Authorize(Roles = "Admin")]
    [Route("[action]")]
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var article = await this._articleService.GetArticle(id);
            return View(article.ToUpdateRequest());
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Handles article edit submission.
    /// </summary>
    /// <param name="request">The article update request data.</param>
    /// <returns>A redirect to the article detail, or the edit view on failure.</returns>
    [Authorize(Roles = "Admin")]
    [Route("[action]")]
    [HttpPost]
    public async Task<IActionResult> Edit(ArticleUpdateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return View(request);
        }

        try
        {
            await this._articleService.UpdateArticle(request);
            return RedirectToAction("Detail", new { id = request.ArticleId });
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}
