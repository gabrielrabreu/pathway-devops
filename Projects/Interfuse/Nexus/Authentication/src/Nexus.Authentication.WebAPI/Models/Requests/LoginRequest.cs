using System.ComponentModel.DataAnnotations;

namespace Nexus.Authentication.WebAPI.Models.Requests
{
    /// <summary>
    /// Model representing the request data for user login.
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// Gets or sets the username for login.
        /// </summary>
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the password for login.
        /// </summary>
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;
    }
}
