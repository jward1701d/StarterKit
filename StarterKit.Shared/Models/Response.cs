using System.Collections.Generic;
using System.Net;

namespace StarterKit.Shared.Models;

/// <summary>
/// A generic wrapper for service results and API endpoint payloads.
/// </summary>
/// <typeparam name="T">The type of the response data.</typeparam>
public sealed class Response<T>
{
    /// <summary>
    /// Gets the HTTP status code representing the outcome of the operation.
    /// </summary>
    public HttpStatusCode StatusCode { get; init; }

    /// <summary>
    /// Gets the human-readable message describing the outcome.
    /// </summary>
    public string Message { get; init; }

    /// <summary>
    /// Gets the response payload, if any.
    /// </summary>
    public T? Data { get; init; }

    /// <summary>
    /// Gets the list of errors associated with a failed operation, if any.
    /// </summary>
    public List<ApiError>? Errors { get; init; }

    /// <summary>
    /// Gets a value indicating whether the operation succeeded (status code 200–299).
    /// </summary>
    public bool IsSuccess => (int)StatusCode >= 200 && (int)StatusCode <= 299;

    private Response(HttpStatusCode statusCode, string message, T? data, List<ApiError>? errors)
    {
        StatusCode = statusCode;
        Message = message;
        Data = data;
        Errors = errors;
    }

    /// <summary>
    /// Creates a successful <see cref="Response{T}"/> with the provided data.
    /// </summary>
    /// <param name="data">The response payload.</param>
    /// <param name="message">The success message.</param>
    /// <param name="statusCode">The HTTP status code. Defaults to <see cref="HttpStatusCode.OK"/>.</param>
    /// <returns>A <see cref="Response{T}"/> representing a successful outcome.</returns>
    public static Response<T> Success(T data, string message = "Success", HttpStatusCode statusCode = HttpStatusCode.OK)
        => new(statusCode, message, data, null);

    /// <summary>
    /// Creates a failed <see cref="Response{T}"/> with the provided error details.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="statusCode">The HTTP status code representing the failure.</param>
    /// <param name="errors">An optional list of <see cref="ApiError"/> instances describing individual errors.</param>
    /// <returns>A <see cref="Response{T}"/> representing a failed outcome.</returns>
    public static Response<T> Failure(string message, HttpStatusCode statusCode, List<ApiError>? errors = null)
        => new(statusCode, message, default, errors);
}
