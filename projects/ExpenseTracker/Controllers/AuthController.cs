using ExpenseTracker.Enums;
using ExpenseTracker.Models.Auth;
using ExpenseTracker.Models.DTOs;
using ExpenseTracker.Services.JWT;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExpenseTracker.Controllers
{
    [ApiController]
    [Route("api/v1/account")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {

        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IJWTService _jwtService;
        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, IJWTService jwtService, RoleManager<Role> roleManager   )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
            _roleManager = roleManager;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user is null)
                return Unauthorized(new { message = "Invalid email or password" });

            if (!await _userManager.CheckPasswordAsync(user, dto.Password))
                return Unauthorized(new { message = "Invalid email or password" });

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
            if (emailInUseResult is BadRequestObjectResult) return emailInUseResult;

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
            var userRole =   UserTypeOptions.User.ToString();
            //se que es mala practica, pero por ahora lo dejo asi, luego se puede mejorar blep:p
            if (!await _roleManager.RoleExistsAsync(userRole))
            {
                await _roleManager.CreateAsync(new Role { Name = userRole });
            }
            

            await _userManager.AddToRoleAsync(user,userRole);

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
            if (string.IsNullOrEmpty(email)) return BadRequest(new { message = "Email is required" });
            var user = await _userManager.FindByEmailAsync(email);
            return Ok(user != null);
        }
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromHeader] TokenDTO tokendto)
        {

            if (tokendto is null || string.IsNullOrEmpty(tokendto.TokenString) || string.IsNullOrEmpty(tokendto.RefreshToken))
                return BadRequest(new { message = "Invalid token" });

            string? token = tokendto.TokenString;

            ClaimsPrincipal ClaimsPrincipal = _jwtService.ValidateJWT(token);

            if (ClaimsPrincipal == null)
                return BadRequest(new { message = "Invalid token" });

            string email = ClaimsPrincipal.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null || user.RefreshToken != tokendto.RefreshToken || user.RefreshTokenExpirationDate <= DateTime.UtcNow)
                return BadRequest(new { message = "Invalid token" });

            var roles = await _userManager.GetRolesAsync(user);
            var authResponse = _jwtService.CreateJWT(user, roles);

            user.RefreshToken = authResponse.RefreshToken;
            user.RefreshTokenExpirationDate = authResponse.RefreshTokenExpirationDate;

            await _userManager.UpdateAsync(user);

            return Ok(authResponse);
        }
}
}
