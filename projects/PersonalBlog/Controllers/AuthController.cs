using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PersonalBlog.Models;
using PersonalBlog.Models.Auth;
using PersonalBlog.Models.DTOs;

namespace PersonalBlog.Controllers;

public class AuthController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    
    public AuthController(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpGet]
    [Route("/login")]
    public IActionResult Index()
    {
        // Si ya está autenticado, redirigir al inicio
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction("Index", "Home");
        
        return View();
    }
    
    [HttpPost]
    [Route("/login")]
    public async Task<IActionResult> Index(LoginDTO dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Errors = ModelState.Values.SelectMany(value => value.Errors).Select(e => e.ErrorMessage).ToList();
            return View(dto);
        }

        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user is null)
        {
            ModelState.AddModelError(string.Empty, "Email o contraseña incorrectos.");
            ViewBag.Errors = new List<string> { "Email o contraseña incorrectos." };
            return View(dto);
        }

        var result = await _signInManager.PasswordSignInAsync(
            user,
            dto.Password,
            isPersistent: dto.RememberMe,
            lockoutOnFailure: false
        );

        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, "Email o contraseña incorrectos.");
            ViewBag.Errors = new List<string> { "Email o contraseña incorrectos." };
            return View(dto);
        }

        // Redirigir según el rol
        if (await _userManager.IsInRoleAsync(user, UserTypeOptions.Admin.ToString()))
            return RedirectToAction("Index", "Admin", new { area = "Admin" });

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [Route("/logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    [Route("/access-denied")]
    public IActionResult AccessDenied() => View();
}