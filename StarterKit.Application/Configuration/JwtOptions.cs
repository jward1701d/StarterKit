namespace StarterKit.Application.Configuration;

/// <summary>
/// Strongly-typed configuration options for JWT token generation and validation.
/// </summary>
public class JwtOptions
{
    /// <summary>
    /// The configuration section name used for binding.
    /// </summary>
    public const string SectionName = "Jwt";

    /// <summary>
    /// Gets or sets the token issuer.
    /// </summary>
    public required string Issuer { get; set; }

    /// <summary>
    /// Gets or sets the token audience.
    /// </summary>
    public required string Audience { get; set; }

    /// <summary>
    /// Gets or sets the signing key used to sign and validate tokens.
    /// </summary>
    public required string SigningKey { get; set; }

    /// <summary>
    /// Gets or sets the access token lifetime in minutes.
    /// </summary>
    public required int AccessTokenMinutes { get; set; }
}
