using StarterKit.Application.Interfaces.Security;

namespace StarterKit.Infrastructure.Security;

/// <summary>
/// Implements password hashing and verification using BCrypt.
/// </summary>
public class PasswordHashingService : IPasswordHashingService
{
    /// <inheritdoc />
    public string HashPassword(string password) =>
        BCrypt.Net.BCrypt.HashPassword(password);

    /// <inheritdoc />
    public bool VerifyPassword(string password, string passwordHash) =>
        BCrypt.Net.BCrypt.Verify(password, passwordHash);
}
