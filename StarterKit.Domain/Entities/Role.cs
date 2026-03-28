using System;
using System.Collections.Generic;

namespace StarterKit.Domain.Entities;

/// <summary>
/// Represents a role that can be assigned to users for authorization purposes.
/// </summary>
public class Role
{
    /// <summary>
    /// Gets or sets the unique identifier for the role.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the role (e.g., "Admin", "User").
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a human-readable description of the role.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the UTC date and time when the role was created.
    /// </summary>
    public DateTime CreatedUtc { get; set; }

    /// <summary>
    /// Gets or sets the users assigned to this role.
    /// </summary>
    public ICollection<UserRole> UserRoles { get; set; } = [];
}
