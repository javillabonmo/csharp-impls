using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace ExpenseTracker.Models.Auth
{
    [CollectionName("Roles")]
    public class Role : MongoIdentityRole<Guid>
    {
        public Role()
            : base()
        {
        }

        public Role(string roleName)
            : base(roleName)
        {
        }
    }
}
