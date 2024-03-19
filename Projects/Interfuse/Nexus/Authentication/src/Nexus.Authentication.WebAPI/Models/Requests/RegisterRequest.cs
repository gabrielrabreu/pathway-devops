using System.ComponentModel.DataAnnotations;

namespace Nexus.Authentication.WebAPI.Models.Requests
{
    /// <summary>
    /// Model representing the request data for user registration.
    /// </summary>
    public class RegisterRequest
    {
        /// <summary>
        /// Gets or sets the username for registration.
        /// </summary>
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the email for registration.
        /// </summary>
        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the password for registration.
        /// </summary>
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;
    }
}
