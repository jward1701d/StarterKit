using System;
using System.Threading;
using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using StarterKit.Application.Interfaces.Auth;
using StarterKit.Contracts.Auth;

namespace StarterKit.Api.Endpoints;

/// <summary>
/// Maps authentication endpoints for the Free version.
/// </summary>
public static class AuthEndpoints
{
    /// <summary>
    /// Registers all authentication routes on the provided <see cref="IEndpointRouteBuilder"/>.
    /// </summary>
    /// <param name="app">The endpoint route builder.</param>
    /// <returns>The updated <see cref="IEndpointRouteBuilder"/>.</returns>
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/api/auth").WithTags("Auth");

        group.MapPost("/register", async (
            [FromBody] RegisterRequest request,
            IAuthService authService,
            CancellationToken cancellationToken) =>
        {
            var response = await authService.RegisterAsync(request, cancellationToken);
            return Results.Json(response, statusCode: (int)response.StatusCode);
        });

        group.MapPost("/login", async (
            [FromBody] LoginRequest request,
            IAuthService authService,
            CancellationToken cancellationToken) =>
        {
            var response = await authService.LoginAsync(request, cancellationToken);
            return Results.Json(response, statusCode: (int)response.StatusCode);
        });

        group.MapGet("/me", async (
            ClaimsPrincipal principal,
            IAuthService authService,
            CancellationToken cancellationToken) =>
        {
            string? userIdValue = principal.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? principal.FindFirstValue("sub");

            if (!Guid.TryParse(userIdValue, out Guid userId))
            {
                return Results.Unauthorized();
            }

            var response = await authService.GetCurrentUserAsync(userId, cancellationToken);
            return Results.Json(response, statusCode: (int)response.StatusCode);
        }).RequireAuthorization();

        return app;
    }
}
