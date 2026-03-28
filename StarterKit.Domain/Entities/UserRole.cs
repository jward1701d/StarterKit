using System;

namespace StarterKit.Domain.Entities;

/// <summary>
/// Represents the many-to-many join between a <see cref="User"/> and a <see cref="Role"/>.
/// </summary>
public class UserRole
{
    /// <summary>
    /// Gets or sets the unique identifier for this assignment.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the assigned user.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the assigned role.
    /// </summary>
    public Guid RoleId { get; set; }

    /// <summary>
    /// Gets or sets the UTC date and time when the role was assigned to the user.
    /// </summary>
    public DateTime AssignedUtc { get; set; }

    /// <summary>
    /// Gets or sets the associated user navigation property.
    /// </summary>
    public User? User { get; set; }

    /// <summary>
    /// Gets or sets the associated role navigation property.
    /// </summary>
    public Role? Role { get; set; }
}
