using ExpenseTracker.Models.Auth;
using ExpenseTracker.Models.DTOs;
using System.Security.Claims;

namespace ExpenseTracker.Services.JWT
{
    public interface IJWTService
    {
        AuthResponseDTO CreateJWT(User user, IList<string> roles);

        ClaimsPrincipal? ValidateJWT(string? token);
    }
}
