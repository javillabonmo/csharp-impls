namespace ExpenseTracker.Models.DTOs
{
    public class AuthResponseDTO
    {
        public string Token { get; set; } = string.Empty;

        public string RefreshToken { get; set; } = string.Empty;

        public DateTime ExpiresAt { get; set; }

        public string UserEmail { get; set; } = string.Empty;

        public DateTime RefreshTokenExpirationDate { get; set; }
    }
}
