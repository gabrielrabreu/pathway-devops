using System.ComponentModel.DataAnnotations;

namespace Nexus.Authentication.WebAPI.Models.Requests
{
    /// <summary>
    /// Model representing the request data for refresh token.
    /// </summary>
    public class RefreshTokenRequest
    {
        /// <summary>
        /// Gets or sets the access token for refresh token.
        /// </summary>
        [Required(ErrorMessage = "Access token is required")]
        public string AccessToken { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the refresh token for refresh token.
        /// </summary>
        [Required(ErrorMessage = "Refresh token is required")]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
