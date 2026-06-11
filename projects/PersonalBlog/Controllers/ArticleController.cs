using Microsoft.AspNetCore.Mvc;

namespace PersonalBlog.Controllers;

public class ArticleController : Controller
{
    [Route("/article/{id}")]
    public IActionResult Detail(int id)
    {
        return View();
    }
}