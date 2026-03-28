using System;
using System.Threading;
using System.Threading.Tasks;
using StarterKit.Contracts.Auth;
using StarterKit.Contracts.Users;
using StarterKit.Shared.Models;

namespace StarterKit.Application.Interfaces.Users;

/// <summary>
/// Defines use cases for managing the current user's profile.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Retrieves a user's profile by their unique identifier.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A <see cref="Response{T}"/> containing the <see cref="CurrentUserResponse"/> on success.</returns>
    Task<Response<CurrentUserResponse>> GetByIdAsync(Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Updates the profile of the specified user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user to update.</param>
    /// <param name="request">The profile update payload.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A <see cref="Response{T}"/> containing the updated <see cref="CurrentUserResponse"/> on success.</returns>
    Task<Response<CurrentUserResponse>> UpdateProfileAsync(Guid userId, UpdateProfileRequest request, CancellationToken cancellationToken);
}
