using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PersonalBlog.Models;

namespace PersonalBlog.Controllers;

public class HomeController : Controller
{
    
    [HttpGet]
    [Route("/home")]
    public IActionResult Index()
    {
        return View();
    }
    

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}