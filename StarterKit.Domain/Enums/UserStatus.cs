namespace StarterKit.Domain.Enums;

/// <summary>
/// Represents the lifecycle status of a user account.
/// </summary>
public enum UserStatus
{
    /// <summary>
    /// The user has registered but has not yet verified their account.
    /// </summary>
    PendingVerification,

    /// <summary>
    /// The user account is active and in good standing.
    /// </summary>
    Active,

    /// <summary>
    /// The user account has been suspended and access is restricted.
    /// </summary>
    Suspended,

    /// <summary>
    /// The user account has been deleted.
    /// </summary>
    Deleted
}
