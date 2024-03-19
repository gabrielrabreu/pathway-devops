using Microsoft.AspNetCore.Identity;

namespace Nexus.Authentication.WebAPI.Data.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}
