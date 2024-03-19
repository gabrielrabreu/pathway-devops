using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Nexus.Authentication.WebAPI.Service.Interfaces
{
    public interface ITokenService
    {
        JwtSecurityToken CreateToken(List<Claim> authClaims);
        string GenerateRefreshToken();
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token);
        DateTime RefreshTokenExpiryTime();
    }
}
