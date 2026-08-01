using System.Security.Claims;
using ExpenseTracker.Enums;
using ExpenseTracker.Models.Auth;
using ExpenseTracker.Models.DTOs;
using ExpenseTracker.Services.JWT;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers;

[ApiController]
[Route("api/v1/account")]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IJWTService _jwtService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, IJWTService jwtService, RoleManager<Role> roleManager, ILogger<AuthController> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtService = jwtService;
        _roleManager = roleManager;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user is null)
        {
            _logger.LogWarning("Login attempt with unknown email {Email}", dto.Email);
            return Unauthorized(new { message = "Invalid email or password" });
        }

        if (!await _userManager.CheckPasswordAsync(user, dto.Password))
        {
            _logger.LogWarning("Failed login attempt for user {Email}", dto.Email);
            return Unauthorized(new { message = "Invalid email or password" });
        }

        var roles = await _userManager.GetRolesAsync(user);
        var authResponse = _jwtService.CreateJWT(user, roles);

        user.RefreshToken = authResponse.RefreshToken;
        user.RefreshTokenExpirationDate = authResponse.RefreshTokenExpirationDate;
        await _userManager.UpdateAsync(user);

        return Ok(authResponse);
    }

    [HttpPost("revoke")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
            return Unauthorized();

        user.RefreshToken = null;
        user.RefreshTokenExpirationDate = DateTime.MinValue;
        await _userManager.UpdateAsync(user);

        return Ok(new { message = "Session revoked successfully" });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var emailInUseResult = await IsEmailInUse(dto.Email);
        if (emailInUseResult is BadRequestObjectResult)
        {
            return emailInUseResult;
        }

        User user = new User { Email = dto.Email, UserName = dto.Email };
        IdentityResult identityResult = await _userManager.CreateAsync(user, dto.Password);

        if (!identityResult.Succeeded)
        {
            foreach (var error in identityResult.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }

            return BadRequest(ModelState);
        }

        var userRole = UserTypeOptions.User.ToString();

        // se que es mala practica, pero por ahora lo dejo asi, luego se puede mejorar blep:p
        if (!await _roleManager.RoleExistsAsync(userRole))
        {
            await _roleManager.CreateAsync(new Role { Name = userRole });
        }

        await _userManager.AddToRoleAsync(user, userRole);

        var roles = await _userManager.GetRolesAsync(user);
        var authResponse = _jwtService.CreateJWT(user, roles);

        user.RefreshToken = authResponse.RefreshToken;
        user.RefreshTokenExpirationDate = authResponse.RefreshTokenExpirationDate;
        await _userManager.UpdateAsync(user);

        return Ok(authResponse);
    }

    [NonAction]
    [AllowAnonymous]
    public async Task<ActionResult> IsEmailInUse(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            return BadRequest(new { message = "Email is required" });
        }

        var user = await _userManager.FindByEmailAsync(email);
        return Ok(user != null);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromHeader] TokenDTO tokendto)
    {
        if (tokendto is null || string.IsNullOrEmpty(tokendto.TokenString) || string.IsNullOrEmpty(tokendto.RefreshToken))
            return BadRequest(new { message = "Invalid token" });

        string? token = tokendto.TokenString;

        ClaimsPrincipal claimsPrincipal = _jwtService.ValidateJWT(token);

        if (claimsPrincipal == null)
            return BadRequest(new { message = "Invalid token" });

        string email = claimsPrincipal.FindFirst(ClaimTypes.Email)?.Value;
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null || user.RefreshToken != tokendto.RefreshToken || user.RefreshTokenExpirationDate <= DateTime.UtcNow)
        {
            _logger.LogWarning("Invalid token refresh attempt for {Email}", email);
            return BadRequest(new { message = "Invalid token" });
        }

        var roles = await _userManager.GetRolesAsync(user);
        var authResponse = _jwtService.CreateJWT(user, roles);

        user.RefreshToken = authResponse.RefreshToken;
        user.RefreshTokenExpirationDate = authResponse.RefreshTokenExpirationDate;

        await _userManager.UpdateAsync(user);

        return Ok(authResponse);
    }
}
