using System;
using System.Collections.Generic;

namespace StarterKit.Contracts.Auth;

/// <summary>
/// Represents the authenticated user's profile information returned in an auth response.
/// </summary>
public class CurrentUserResponse
{
    /// <summary>
    /// Gets or sets the unique identifier of the user.
    /// </summary>
    public required Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the user's email address.
    /// </summary>
    public required string Email { get; set; }

    /// <summary>
    /// Gets or sets the user's display name.
    /// </summary>
    public required string DisplayName { get; set; }

    /// <summary>
    /// Gets or sets the list of role names assigned to the user.
    /// </summary>
    public required List<string> Roles { get; set; }
}
