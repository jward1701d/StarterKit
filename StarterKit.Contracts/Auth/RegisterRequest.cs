namespace StarterKit.Contracts.Auth;

/// <summary>
/// Represents the input payload for a user registration request.
/// </summary>
public class RegisterRequest
{
    /// <summary>
    /// Gets or sets the email address for the new account.
    /// </summary>
    public required string Email { get; set; }

    /// <summary>
    /// Gets or sets the password for the new account.
    /// </summary>
    public required string Password { get; set; }

    /// <summary>
    /// Gets or sets the display name shown in the application.
    /// </summary>
    public required string DisplayName { get; set; }
}
