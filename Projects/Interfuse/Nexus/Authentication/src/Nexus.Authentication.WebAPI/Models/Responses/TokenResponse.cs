namespace Nexus.Authentication.WebAPI.Models.Responses
{
    /// <summary>
    /// Model representing the response data for token generation.
    /// </summary>
    public class TokenResponse
    {
        /// <summary>
        /// Gets or sets the access token.
        /// </summary>
        public string AccessToken { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the refresh token.
        /// </summary>
        public string RefreshToken { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the access token expiration.
        /// </summary>
        public DateTime Expiration { get; set; } = DateTime.MinValue;

        /// <summary>
        /// Gets or sets the user information associated with the token.
        /// </summary>
        public TokenUserResponse? User { get; set; } = new();
    }
}
