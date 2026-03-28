using System;
using System.Threading;
using System.Threading.Tasks;
using StarterKit.Contracts.Auth;
using StarterKit.Shared.Models;

namespace StarterKit.Application.Interfaces.Auth;

/// <summary>
/// Defines the authentication use cases for the application.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Registers a new user account.
    /// </summary>
    /// <param name="request">The registration payload.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A <see cref="Response{T}"/> containing the <see cref="AuthResponse"/> on success.</returns>
    Task<Response<AuthResponse>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Authenticates a user and issues an access token.
    /// </summary>
    /// <param name="request">The login payload.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A <see cref="Response{T}"/> containing the <see cref="AuthResponse"/> on success.</returns>
    Task<Response<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves the profile of the currently authenticated user.
    /// </summary>
    /// <param name="userId">The unique identifier of the authenticated user.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A <see cref="Response{T}"/> containing the <see cref="CurrentUserResponse"/> on success.</returns>
    Task<Response<CurrentUserResponse>> GetCurrentUserAsync(Guid userId, CancellationToken cancellationToken);
}
