namespace StarterKit.Contracts.Users;

/// <summary>
/// Represents the input payload for updating the current user's profile.
/// </summary>
public class UpdateProfileRequest
{
    /// <summary>
    /// Gets or sets the new display name for the user.
    /// </summary>
    public required string DisplayName { get; set; }
}
