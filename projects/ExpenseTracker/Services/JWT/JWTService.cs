using ExpenseTracker.Models.Auth;
using ExpenseTracker.Models.DTOs;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ExpenseTracker.Services.JWT
{

    public class JWTService : IJWTService
    {
        private readonly IConfiguration _configuration;

        public JWTService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public AuthResponseDTO CreateJWT(User user, IList<string> roles)
        {
            DateTime expirationTime = DateTime.UtcNow.AddMinutes(
                Convert.ToDouble(_configuration["JWT:ExpiresInMinutes"]));

            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Email),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString()),
        };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken JwtSecurityToken = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims: claims,
                expires: expirationTime,
                signingCredentials: signingCredentials
            );

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            string token = tokenHandler.WriteToken(JwtSecurityToken);

            return new AuthResponseDTO
            {
                Token = token,
                ExpiresAt = expirationTime,
                RefreshToken = CreateRefreshToken(),
                UserEmail = user.Email,
                RefreshTokenExpirationDate = DateTime.UtcNow.AddMinutes(
                    Convert.ToDouble(_configuration["RefreshToken:ExpiresInMinutes"]))
            };
        }

        public string CreateRefreshToken()
        {
            byte[] bytes = new byte[64];
            var random = RandomNumberGenerator.Create();

            random.GetBytes(bytes);

            return Convert.ToBase64String(bytes);
        }
        public ClaimsPrincipal? ValidateJWT(string? token)
        {
            if (string.IsNullOrEmpty(token))
                return null;

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _configuration["JWT:Issuer"],
                ValidAudience = _configuration["JWT:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"])),
                ClockSkew = TimeSpan.Zero
            };

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            ClaimsPrincipal ClaimsPrincipal = jwtSecurityTokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);

            if (validatedToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }
            return ClaimsPrincipal;
        }
    }
}
