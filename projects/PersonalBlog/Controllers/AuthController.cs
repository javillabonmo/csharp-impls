// <copyright file="AuthController.cs" company="TBRZCom">
// Copyright (c) TBRZCom. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using PersonalBlog.Models;
using PersonalBlog.Models.Auth;
using PersonalBlog.Models.DTOs;

namespace PersonalBlog.Controllers;

/// <summary>
/// Controller for authentication operations.
/// </summary>
public class AuthController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthController"/> class.
    /// </summary>
    /// <param name="userManager">The user manager.</param>
    /// <param name="signInManager">The sign-in manager.</param>
    public AuthController(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        this._userManager = userManager;
        this._signInManager = signInManager;
    }

    /// <summary>
    /// Displays the login page.
    /// </summary>
    /// <returns>The login view.</returns>
    [HttpGet]
    [Route("/login")]
    [EnableRateLimiting("Login")]
    public IActionResult Index()
    {
        // Si ya está autenticado, redirigir al inicio
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction("Index", "Home");
        }

        return View();
    }

    /// <summary>
    /// Handles login form submission.
    /// </summary>
    /// <param name="dto">The login data transfer object.</param>
    /// <returns>A redirect to the appropriate home page, or the login view on failure.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("/login")]
    public async Task<IActionResult> Index(LoginDTO dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Errors = ModelState.Values.SelectMany(value => value.Errors).Select(e => e.ErrorMessage).ToList();
            return View(dto);
        }

        var user = await this._userManager.FindByEmailAsync(dto.Email);
        if (user is null)
        {
            ModelState.AddModelError(string.Empty, "Email o contraseña incorrectos.");
            ViewBag.Errors = new List<string> { "Email o contraseña incorrectos." };
            return View(dto);
        }

        var result = await this._signInManager.PasswordSignInAsync(
            user,
            dto.Password,
            isPersistent: dto.RememberMe,
            lockoutOnFailure: true);

        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, "Email o contraseña incorrectos.");
            ViewBag.Errors = new List<string> { "Email o contraseña incorrectos." };
            return View(dto);
        }

        // Redirigir según el rol
        if (await this._userManager.IsInRoleAsync(user, UserTypeOptions.Admin.ToString()))
        {
            return RedirectToAction("Index", "Admin", new { area = "Admin" });
        }

        return RedirectToAction("Index", "Home");
    }

    /// <summary>
    /// Logs the user out.
    /// </summary>
    /// <returns>A redirect to the home page.</returns>
    [HttpPost]
    [Route("/logout")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await this._signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    /// <summary>
    /// Displays the access denied page.
    /// </summary>
    /// <returns>The access denied view.</returns>
    [HttpGet]
    [Route("/access-denied")]
    public IActionResult AccessDenied() => View();
}
