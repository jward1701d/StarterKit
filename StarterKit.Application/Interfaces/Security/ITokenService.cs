using System.Collections.Generic;
using StarterKit.Domain.Entities;

namespace StarterKit.Application.Interfaces.Security;

/// <summary>
/// Abstracts JWT access token generation.
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// Generates a signed JWT access token for the specified user and roles.
    /// </summary>
    /// <param name="user">The authenticated user for whom the token is generated.</param>
    /// <param name="roles">The roles to embed as claims in the token.</param>
    /// <returns>A signed JWT access token string.</returns>
    string GenerateAccessToken(User user, IReadOnlyCollection<string> roles);
}
