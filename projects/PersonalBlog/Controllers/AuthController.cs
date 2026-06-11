using Microsoft.AspNetCore.Mvc;

namespace PersonalBlog.Controllers;

public class AuthController : Controller
{
    [HttpGet]
    [Route("/login")]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    [Route("[action]")]
    public IActionResult Logout()
    {
        return View();
    }
}