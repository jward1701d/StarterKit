namespace StarterKit.Application.Interfaces.Security;

/// <summary>
/// Abstracts password hashing and verification operations.
/// </summary>
public interface IPasswordHashingService
{
    /// <summary>
    /// Hashes the specified plain-text password.
    /// </summary>
    /// <param name="password">The plain-text password to hash.</param>
    /// <returns>The hashed representation of the password.</returns>
    string HashPassword(string password);

    /// <summary>
    /// Verifies a plain-text password against a stored password hash.
    /// </summary>
    /// <param name="password">The plain-text password to verify.</param>
    /// <param name="passwordHash">The stored password hash to verify against.</param>
    /// <returns><see langword="true"/> if the password matches the hash; otherwise, <see langword="false"/>.</returns>
    bool VerifyPassword(string password, string passwordHash);
}
