using System;
using System.Collections.Generic;
using StarterKit.Domain.Enums;

namespace StarterKit.Domain.Entities;

/// <summary>
/// Represents an application user for authentication and authorization.
/// </summary>
public class User
{
    /// <summary>
    /// Gets or sets the unique identifier for the user.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the user's email address.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the normalized (uppercased) email address used for lookups.
    /// </summary>
    public string NormalizedEmail { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the hashed password for the user.
    /// </summary>
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the display name shown in the application.
    /// </summary>
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the lifecycle status of the user account.
    /// </summary>
    public UserStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the UTC date and time when the user was created.
    /// </summary>
    public DateTime CreatedUtc { get; set; }

    /// <summary>
    /// Gets or sets the UTC date and time when the user was last updated.
    /// </summary>
    public DateTime UpdatedUtc { get; set; }

    /// <summary>
    /// Gets or sets the UTC date and time of the user's last successful login, if any.
    /// </summary>
    public DateTime? LastLoginUtc { get; set; }

    /// <summary>
    /// Gets or sets the token version used to invalidate issued tokens on demand.
    /// </summary>
    public int TokenVersion { get; set; }

    /// <summary>
    /// Gets or sets the roles assigned to the user.
    /// </summary>
    public ICollection<UserRole> UserRoles { get; set; } = [];
}
