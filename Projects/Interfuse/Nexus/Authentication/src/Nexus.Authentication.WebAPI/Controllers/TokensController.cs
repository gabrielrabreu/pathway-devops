using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nexus.Authentication.WebAPI.Data.Entities;
using Nexus.Authentication.WebAPI.Exceptions;
using Nexus.Authentication.WebAPI.Models.Requests;
using Nexus.Authentication.WebAPI.Models.Responses;
using Nexus.Authentication.WebAPI.Service.Interfaces;
using System.IdentityModel.Tokens.Jwt;

namespace Nexus.Authentication.WebAPI.Controllers
{
    /// <summary>
    /// Controller responsible for authentication token operations.
    /// </summary>
    public class TokensController(UserManager<ApplicationUser> userManager,
                                  ITokenService tokenService) : BaseController
    {
        /// <summary>
        /// Refreshes access token using refresh token.
        /// </summary>
        /// <param name="request">Refresh token request containing access and refresh tokens.</param>
        /// <returns>Returns token response if refresh is successful.</returns>
        /// <response code="200">Refresh successful, returns token response.</response>
        /// <response code="401">Unauthorized access, invalid tokens.</response>
        [HttpPost("refresh")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(TokenResponse), 200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequest request)
        {
            if (request is null)
                throw new UnauthorizedAccessException();

            string? accessToken = request.AccessToken;
            string? refreshToken = request.RefreshToken;

            var principal = tokenService.GetPrincipalFromExpiredToken(accessToken);

            if (principal == null || principal.Identity == null || principal.Identity.Name == null)
                throw new UnauthorizedAccessException();

            string username = principal.Identity.Name;

            var user = await userManager.FindByNameAsync(username);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                throw new UnauthorizedAccessException();

            var newAccessToken = tokenService.CreateToken(principal.Claims.ToList());
            var newRefreshToken = tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await userManager.UpdateAsync(user);

            return Ok(new TokenResponse
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                RefreshToken = newRefreshToken,
                Expiration = newAccessToken.ValidTo,
                User = new TokenUserResponse
                {
                    Username = username
                }
            });
        }

        /// <summary>
        /// Registers an administrator.
        /// Revokes refresh token for a specific user.
        /// </summary>
        /// <param name="username">Username of the user.</param>
        /// <returns>No content if token is revoked successfully.</returns>
        /// <response code="204">Token revoked successfully.</response>
        /// <response code="422">Invalid username.</response>
        [HttpPost("revoke/{username}")]
        [Authorize(Roles = ApplicationRole.Admin)]
        [ProducesResponseType(204)]
        [ProducesResponseType(422)]
        public async Task<IActionResult> Revoke(string username)
        {
            var user = await userManager.FindByNameAsync(username) ?? throw new BusinessRuleViolationException("Invalid username");
            user.RefreshToken = null;
            await userManager.UpdateAsync(user);

            return NoContent();
        }

        /// <summary>
        /// Registers an administrator.
        /// Revokes refresh tokens for all users.
        /// </summary>
        /// <returns>No content if tokens are revoked successfully.</returns>
        /// <response code="204">Tokens revoked successfully for all users.</response>
        [HttpPost("revoke-all")]
        [Authorize(Roles = ApplicationRole.Admin)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> RevokeAll()
        {
            var users = userManager.Users.ToList();

            foreach (var user in users)
            {
                user.RefreshToken = null;
                await userManager.UpdateAsync(user);
            }

            return NoContent();
        }
    }
}
