using Microsoft.AspNetCore.Identity;

namespace Nexus.Authentication.WebAPI.Data.Entities
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public const string Admin = "Admin";
        public const string User = "User";

        public ApplicationRole()
        {
        }

        public ApplicationRole(string roleName) : base(roleName)
        {
        }
    }
}
