using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nexus.Authentication.WebAPI.Data.Entities;
using Nexus.Authentication.WebAPI.Exceptions;
using Nexus.Authentication.WebAPI.Models.Requests;
using Nexus.Authentication.WebAPI.Models.Responses;
using Nexus.Authentication.WebAPI.Service.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Nexus.Authentication.WebAPI.Controllers
{
    /// <summary>
    /// Controller responsible for authentication operations.
    /// </summary>
    public class AccountsController(UserManager<ApplicationUser> userManager,
                                    ITokenService tokenService) : BaseController
    {
        /// <summary>
        /// Logs in a user.
        /// </summary>
        /// <param name="request">Login request containing user credentials.</param>
        /// <returns>Returns token response if login is successful.</returns>
        /// <response code="200">Login successful, returns token response.</response>
        /// <response code="401">Unauthorized access, invalid credentials.</response>
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(TokenResponse), 200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var user = await userManager.FindByNameAsync(request.Username);

            if (user == null || user.UserName == null || !await userManager.CheckPasswordAsync(user, request.Password))
                throw new UnauthorizedAccessException();

            var userRoles = await userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new(ClaimTypes.Name, user.UserName)
            };

            foreach (var userRole in userRoles)
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));

            var token = tokenService.CreateToken(authClaims);
            var refreshToken = tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = tokenService.RefreshTokenExpiryTime();

            await userManager.UpdateAsync(user);

            return Ok(new TokenResponse
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken,
                Expiration = token.ValidTo,
                User = new TokenUserResponse
                {
                    Username = user.UserName
                }
            });
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="request">Registration request containing user details.</param>
        /// <returns>No content if registration is successful.</returns>
        /// <response code="204">User registered successfully.</response>
        /// <response code="422">Invalid registration request or user already exists.</response>
        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(204)]
        [ProducesResponseType(422)]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var userExists = await userManager.FindByNameAsync(request.Username);

            if (userExists != null)
                throw new BusinessRuleViolationException("User already exists");

            ApplicationUser user = new()
            {
                Email = request.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = request.Username
            };

            var result = await userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
                throw new BusinessRuleViolationException("User creation failed! Please check user details and try again");

            return NoContent();
        }

        /// <summary>
        /// Registers an administrator.
        /// </summary>
        /// <param name="request">Registration request containing user details.</param>
        /// <returns>No content if registration is successful.</returns>
        /// <response code="204">Admin registered successfully.</response>
        /// <response code="422">Invalid registration request or user already exists.</response>
        [HttpPost("register-admin")]
        [Authorize(Roles = ApplicationRole.Admin)]
        [ProducesResponseType(204)]
        [ProducesResponseType(422)]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterRequest request)
        {
            var userExists = await userManager.FindByNameAsync(request.Username);

            if (userExists != null)
                throw new BusinessRuleViolationException("User already exists");

            ApplicationUser user = new()
            {
                Email = request.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = request.Username
            };

            var result = await userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
                throw new BusinessRuleViolationException("User creation failed! Please check user details and try again");

            await userManager.AddToRoleAsync(user, ApplicationRole.Admin);
            await userManager.AddToRoleAsync(user, ApplicationRole.User);

            return NoContent();
        }
    }
}
