namespace StarterKit.Application.Configuration;

/// <summary>
/// Strongly-typed configuration options for the database connection.
/// </summary>
public class DatabaseOptions
{
    /// <summary>
    /// The configuration section name used for binding.
    /// </summary>
    public const string SectionName = "Database";

    /// <summary>
    /// Gets or sets the database connection string.
    /// </summary>
    public required string ConnectionString { get; set; }
}
