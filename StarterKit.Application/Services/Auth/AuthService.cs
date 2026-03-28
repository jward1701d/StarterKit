using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StarterKit.Application.Interfaces.Auth;
using StarterKit.Application.Interfaces.Security;
using StarterKit.Contracts.Auth;
using StarterKit.Domain.Entities;
using StarterKit.Domain.Enums;
using StarterKit.Application.Persistence;
using StarterKit.Shared.Models;

namespace StarterKit.Application.Services.Auth;

/// <summary>
/// Implements authentication use cases: registration, login, and current-user retrieval.
/// </summary>
public class AuthService : IAuthService
{
    private readonly AppDbContext _db;
    private readonly IPasswordHashingService _passwordHashingService;
    private readonly ITokenService _tokenService;
    private readonly ILogger<AuthService> _logger;

    /// <summary>
    /// Initializes a new instance of <see cref="AuthService"/>.
    /// </summary>
    /// <param name="db">The application database context.</param>
    /// <param name="passwordHashingService">The password hashing service.</param>
    /// <param name="tokenService">The JWT token service.</param>
    /// <param name="logger">The logger.</param>
    public AuthService(
        AppDbContext db,
        IPasswordHashingService passwordHashingService,
        ITokenService tokenService,
        ILogger<AuthService> logger)
    {
        _db = db;
        _passwordHashingService = passwordHashingService;
        _tokenService = tokenService;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<Response<AuthResponse>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken)
    {
        try
        {
            string normalizedEmail = request.Email.ToUpperInvariant();

            bool emailTaken = await _db.Users
                .AnyAsync(u => u.NormalizedEmail == normalizedEmail, cancellationToken);

            if (emailTaken)
            {
                return Response<AuthResponse>.Failure(
                    "An account with this email address already exists.",
                    HttpStatusCode.Conflict,
                    [new ApiError("EMAIL_TAKEN", "An account with this email address already exists.", "Email")]);
            }

            User user = new()
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                NormalizedEmail = normalizedEmail,
                PasswordHash = _passwordHashingService.HashPassword(request.Password),
                DisplayName = request.DisplayName,
                Status = UserStatus.Active,
                CreatedUtc = DateTime.UtcNow,
                UpdatedUtc = DateTime.UtcNow,
                TokenVersion = 0
            };

            Role? userRole = await _db.Roles
                .FirstOrDefaultAsync(r => r.Name == nameof(RoleType.User), cancellationToken);

            if (userRole is not null)
            {
                user.UserRoles.Add(new UserRole
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    RoleId = userRole.Id,
                    AssignedUtc = DateTime.UtcNow
                });
            }

            _db.Users.Add(user);
            await _db.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("User {UserId} registered successfully.", user.Id);

            List<string> roles = userRole is not null ? [userRole.Name] : [];
            string accessToken = _tokenService.GenerateAccessToken(user, roles);

            AuthResponse authResponse = new()
            {
                AccessToken = accessToken,
                AccessTokenExpiresUtc = DateTime.UtcNow,
                User = MapCurrentUserResponse(user, roles)
            };

            return Response<AuthResponse>.Success(authResponse, "Registration successful.", HttpStatusCode.Created);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during registration.");
            return Response<AuthResponse>.Failure("An unexpected error occurred.", HttpStatusCode.InternalServerError);
        }
    }

    /// <inheritdoc />
    public async Task<Response<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        try
        {
            string normalizedEmail = request.Email.ToUpperInvariant();

            User? user = await _db.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail, cancellationToken);

            if (user is null || !_passwordHashingService.VerifyPassword(request.Password, user.PasswordHash))
            {
                return Response<AuthResponse>.Failure(
                    "Invalid email or password.",
                    HttpStatusCode.Unauthorized,
                    [new ApiError("INVALID_CREDENTIALS", "Invalid email or password.")]);
            }

            if (user.Status == UserStatus.Deleted)
            {
                return Response<AuthResponse>.Failure(
                    "Invalid email or password.",
                    HttpStatusCode.Unauthorized,
                    [new ApiError("INVALID_CREDENTIALS", "Invalid email or password.")]);
            }

            if (user.Status == UserStatus.Suspended)
            {
                return Response<AuthResponse>.Failure(
                    "This account has been suspended.",
                    HttpStatusCode.Forbidden,
                    [new ApiError("ACCOUNT_SUSPENDED", "This account has been suspended.")]);
            }

            user.LastLoginUtc = DateTime.UtcNow;
            await _db.SaveChangesAsync(cancellationToken);

            List<string> roles = user.UserRoles
                .Where(ur => ur.Role is not null)
                .Select(ur => ur.Role!.Name)
                .ToList();

            string accessToken = _tokenService.GenerateAccessToken(user, roles);

            _logger.LogInformation("User {UserId} logged in successfully.", user.Id);

            AuthResponse authResponse = new()
            {
                AccessToken = accessToken,
                AccessTokenExpiresUtc = DateTime.UtcNow,
                User = MapCurrentUserResponse(user, roles)
            };

            return Response<AuthResponse>.Success(authResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during login.");
            return Response<AuthResponse>.Failure("An unexpected error occurred.", HttpStatusCode.InternalServerError);
        }
    }

    /// <inheritdoc />
    public async Task<Response<CurrentUserResponse>> GetCurrentUserAsync(Guid userId, CancellationToken cancellationToken)
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

            List<string> roles = user.UserRoles
                .Where(ur => ur.Role is not null)
                .Select(ur => ur.Role!.Name)
                .ToList();

            return Response<CurrentUserResponse>.Success(MapCurrentUserResponse(user, roles));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving user {UserId}.", userId);
            return Response<CurrentUserResponse>.Failure("An unexpected error occurred.", HttpStatusCode.InternalServerError);
        }
    }

    private static CurrentUserResponse MapCurrentUserResponse(User user, List<string> roles) => new()
    {
        Id = user.Id,
        Email = user.Email,
        DisplayName = user.DisplayName,
        Roles = roles
    };
}
