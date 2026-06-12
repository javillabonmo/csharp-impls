using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PersonalBlog.Controllers;
[Authorize]
public class ArticleController : Controller
{
    [Route("/article/{id}")]
    public IActionResult Detail(int id)
    {
        return View();
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
    [HttpGet]
    public IActionResult Edit(int id)
    {
        return View();
    }
}