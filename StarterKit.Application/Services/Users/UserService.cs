using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StarterKit.Application.Interfaces.Users;
using StarterKit.Contracts.Auth;
using StarterKit.Contracts.Users;
using StarterKit.Domain.Entities;
using StarterKit.Application.Persistence;
using StarterKit.Shared.Models;

namespace StarterKit.Application.Services.Users;

/// <summary>
/// Implements current-user profile use cases.
/// </summary>
public class UserService : IUserService
{
    private readonly AppDbContext _db;
    private readonly ILogger<UserService> _logger;

    /// <summary>
    /// Initializes a new instance of <see cref="UserService"/>.
    /// </summary>
    /// <param name="db">The application database context.</param>
    /// <param name="logger">The logger.</param>
    public UserService(AppDbContext db, ILogger<UserService> logger)
    {
        _db = db;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<Response<CurrentUserResponse>> GetByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        try
        {
            User? user = await _db.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

            if (user is null)
            {
                return Response<CurrentUserResponse>.Failure(
                    "User not found.",
                    HttpStatusCode.NotFound,
                    [new ApiError("USER_NOT_FOUND", "User not found.")]);
            }

            return Response<CurrentUserResponse>.Success(MapCurrentUserResponse(user));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving user {UserId}.", userId);
            return Response<CurrentUserResponse>.Failure("An unexpected error occurred.", HttpStatusCode.InternalServerError);
        }
    }

    /// <inheritdoc />
    public async Task<Response<CurrentUserResponse>> UpdateProfileAsync(Guid userId, UpdateProfileRequest request, CancellationToken cancellationToken)
    {
        try
        {
            User? user = await _db.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

            if (user is null)
            {
                return Response<CurrentUserResponse>.Failure(
                    "User not found.",
                    HttpStatusCode.NotFound,
                    [new ApiError("USER_NOT_FOUND", "User not found.")]);
            }

            user.DisplayName = request.DisplayName;
            user.UpdatedUtc = DateTime.UtcNow;

            await _db.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("User {UserId} updated their profile.", userId);

            return Response<CurrentUserResponse>.Success(MapCurrentUserResponse(user));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating profile for user {UserId}.", userId);
            return Response<CurrentUserResponse>.Failure("An unexpected error occurred.", HttpStatusCode.InternalServerError);
        }
    }

    private static CurrentUserResponse MapCurrentUserResponse(User user) => new()
    {
        Id = user.Id,
        Email = user.Email,
        DisplayName = user.DisplayName,
        Roles = user.UserRoles
            .Where(ur => ur.Role is not null)
            .Select(ur => ur.Role!.Name)
            .ToList()
    };
}
