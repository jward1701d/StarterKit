using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace StarterKit.Shared.Models;

/// <summary>
/// Represents a structured error returned as part of an API response.
/// </summary>
public sealed class ApiError
{
    /// <summary>
    /// Gets or sets the machine-readable error code (e.g., "VALIDATION_ERROR", "NOT_FOUND").
    /// </summary>
    [JsonPropertyName("code")]
    public required string Code { get; set; }

    /// <summary>
    /// Gets or sets the human-readable error message.
    /// </summary>
    [JsonPropertyName("message")]
    public required string Message { get; set; }

    /// <summary>
    /// Gets or sets the name of the field associated with the error, if applicable.
    /// </summary>
    [JsonPropertyName("field")]
    public string? Field { get; set; }

    /// <summary>
    /// Initializes a new instance of <see cref="ApiError"/>.
    /// </summary>
    public ApiError() { }

    /// <summary>
    /// Initializes a new instance of <see cref="ApiError"/> with the specified values.
    /// </summary>
    /// <param name="code">The machine-readable error code.</param>
    /// <param name="message">The human-readable error message.</param>
    /// <param name="field">The name of the field associated with the error, if applicable.</param>
    [SetsRequiredMembers]
    public ApiError(string code, string message, string? field = null)
    {
        Code = code;
        Message = message;
        Field = field;
    }
}
