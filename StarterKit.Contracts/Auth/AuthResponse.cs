using System;

namespace StarterKit.Contracts.Auth;

/// <summary>
/// Represents the payload returned upon successful authentication.
/// </summary>
public class AuthResponse
{
    /// <summary>
    /// Gets or sets the JWT access token.
    /// </summary>
    public required string AccessToken { get; set; }

    /// <summary>
    /// Gets or sets the UTC date and time at which the access token expires.
    /// </summary>
    public required DateTime AccessTokenExpiresUtc { get; set; }

    /// <summary>
    /// Gets or sets the authenticated user's profile information.
    /// </summary>
    public required CurrentUserResponse User { get; set; }
}
