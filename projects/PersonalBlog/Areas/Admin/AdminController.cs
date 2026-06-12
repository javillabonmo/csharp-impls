using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalBlog.Models;

namespace PersonalBlog.Areas.Admin;
[Area("Admin")]
[Authorize(Roles = nameof(UserTypeOptions.Admin))]

public class AdminController : Controller
{
    [Route("/admin")]
    public IActionResult Index()
    {
        return View();
    }
}