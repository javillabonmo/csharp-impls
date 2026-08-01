using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace ExpenseTracker.Models.Auth
{
    [CollectionName("Usuarios")]

    public class User : MongoIdentityUser<Guid>
    {
        public User()
            : base()
        {
        }

        public User(string userName, string email)
            : base(userName, email)
        {
        }

        public string? RefreshToken { get; set; }

        public DateTime RefreshTokenExpirationDate { get; set; }
    }
}
