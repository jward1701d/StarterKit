using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StarterKit.Application.Interfaces.Auth;
using StarterKit.Application.Interfaces.Security;
using StarterKit.Application.Interfaces.Users;
using StarterKit.Application.Services.Auth;
using StarterKit.Application.Services.Users;
using StarterKit.Application.Configuration;
using StarterKit.Application.Persistence;
using StarterKit.Infrastructure.Security;

namespace StarterKit.Api.Extensions;

/// <summary>
/// Extension methods for registering application services into the DI container.
/// </summary>
public static class ServiceExtensions
{
    /// <summary>
    /// Registers all StarterKit services, options, and database context.
    /// </summary>
    /// <param name="services">The service collection to add registrations to.</param>
    /// <param name="configuration">The application configuration.</param>
    /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddStarterKitServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
        services.Configure<DatabaseOptions>(configuration.GetSection(DatabaseOptions.SectionName));

        string connectionString = configuration[$"{DatabaseOptions.SectionName}:{nameof(DatabaseOptions.ConnectionString)}"]
            ?? throw new InvalidOperationException("Database connection string is not configured.");

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IPasswordHashingService, PasswordHashingService>();
        services.AddScoped<ITokenService, JwtTokenService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}
