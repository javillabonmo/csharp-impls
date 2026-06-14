using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalBlog.Models.DTOs;
using PersonalBlog.Services;

namespace PersonalBlog.Controllers;


public class ArticleController : Controller
{
    private readonly IArticleService _articleService;

    public ArticleController(IArticleService articleService)
    {
        _articleService = articleService;
    }

    [Route("/article/{id}")]
    public async Task<IActionResult> Detail(int id)
    {
        try
        {
            var article = await _articleService.GetArticle(id);
            return View(article);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [Authorize(Roles = "Admin")]
    [Route("[action]")]
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [Authorize(Roles = "Admin")]
    [Route("[action]")]
    [HttpPost]
    public async Task<IActionResult> Create(ArticleRequest request)
    {
        if (!ModelState.IsValid)
            return View(request);

        try
        {
            var article = await _articleService.AddArticle(request);
            return RedirectToAction("Detail", new { id = article.Id });
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(request);
        }
    }

    [Authorize(Roles = "Admin")]
    [Route("[action]")]
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var article = await _articleService.GetArticle(id);
            return View(article.ToUpdateRequest());
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [Authorize(Roles = "Admin")]
    [Route("[action]")]
    [HttpPost]
    public async Task<IActionResult> Edit(ArticleUpdateRequest request)
    {
        if (!ModelState.IsValid)
            return View(request);

        try
        {
            await _articleService.UpdateArticle(request);
            return RedirectToAction("Detail", new { id = request.ArticleId });
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}
