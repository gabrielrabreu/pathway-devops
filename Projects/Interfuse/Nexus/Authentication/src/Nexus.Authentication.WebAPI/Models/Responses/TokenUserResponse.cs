namespace Nexus.Authentication.WebAPI.Models.Responses
{
    /// <summary>
    /// Model representing the user information in the token response.
    /// </summary>
    public class TokenUserResponse
    {
        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; } = string.Empty;
    }
}
