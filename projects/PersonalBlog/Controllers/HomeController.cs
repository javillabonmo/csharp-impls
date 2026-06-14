using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PersonalBlog.Models;
using PersonalBlog.Models.DTOs;
using PersonalBlog.Services;

namespace PersonalBlog.Controllers;

public class HomeController : Controller
{
    private readonly IArticleService _articleService;

    public HomeController(IArticleService articleService)
    {
        _articleService = articleService;
    }
    [HttpGet]
    [Route("/")]
    public async Task<IActionResult> Root()
    {
        return RedirectToAction("Index");
    }

    [HttpGet]
    [Route("/home")]
    public async Task<IActionResult> Index()
    {
        var articles = await _articleService.GetArticles();
        return View(articles);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
