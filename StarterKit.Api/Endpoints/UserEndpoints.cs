using System;
using System.Threading;
using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using StarterKit.Application.Interfaces.Users;
using StarterKit.Contracts.Users;

namespace StarterKit.Api.Endpoints;

/// <summary>
/// Maps user profile endpoints for the Free version.
/// </summary>
public static class UserEndpoints
{
    /// <summary>
    /// Registers all user profile routes on the provided <see cref="IEndpointRouteBuilder"/>.
    /// </summary>
    /// <param name="app">The endpoint route builder.</param>
    /// <returns>The updated <see cref="IEndpointRouteBuilder"/>.</returns>
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/api/users")
            .WithTags("Users")
            .RequireAuthorization();

        group.MapGet("/me", async (
            ClaimsPrincipal principal,
            IUserService userService,
            CancellationToken cancellationToken) =>
        {
            string? userIdValue = principal.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? principal.FindFirstValue("sub");

            if (!Guid.TryParse(userIdValue, out Guid userId))
            {
                return Results.Unauthorized();
            }

            var response = await userService.GetByIdAsync(userId, cancellationToken);
            return Results.Json(response, statusCode: (int)response.StatusCode);
        });

        group.MapPut("/me", async (
            [FromBody] UpdateProfileRequest request,
            ClaimsPrincipal principal,
            IUserService userService,
            CancellationToken cancellationToken) =>
        {
            string? userIdValue = principal.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? principal.FindFirstValue("sub");

            if (!Guid.TryParse(userIdValue, out Guid userId))
            {
                return Results.Unauthorized();
            }

            var response = await userService.UpdateProfileAsync(userId, request, cancellationToken);
            return Results.Json(response, statusCode: (int)response.StatusCode);
        });

        return app;
    }
}
