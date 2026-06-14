using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalBlog.Models;
using PersonalBlog.Models.DTOs;
using PersonalBlog.Services;

namespace PersonalBlog.Areas.Admin;

[Area("Admin")]
[Authorize(Roles = nameof(UserTypeOptions.Admin))]
public class AdminController : Controller
{
    private readonly IArticleService _articleService;

    public AdminController(IArticleService articleService)
    {
        _articleService = articleService;
    }

    [Route("/admin")]
    public async Task<IActionResult> Index()
    {
        var articles = await _articleService.GetArticles();
        return View(articles);
    }
}
