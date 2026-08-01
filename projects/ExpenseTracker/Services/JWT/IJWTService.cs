using System.Security.Claims;
using ExpenseTracker.Models.Auth;
using ExpenseTracker.Models.DTOs;

namespace ExpenseTracker.Services.JWT
{
    public interface IJWTService
    {
        AuthResponseDTO CreateJWT(User user, IList<string> roles);

        ClaimsPrincipal? ValidateJWT(string? token);
    }
}
