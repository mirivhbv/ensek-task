namespace MeterReadingApi.Core.Infrastructure.Config;

public class DatabaseSettingsModel
{
    /// <summary>
    /// Database type.
    /// </summary>
    /// <remarks>
    /// Supported values are "Npgsql", and "SQLite".
    /// </remarks>
    public required string DbType { get; set; }

    /// <summary>
    /// Connection string for the database.
    /// </summary>
    /// <remarks>
    /// It should be set when <see cref="DbType"/> is set to "Npgsql".
    /// </remarks>
    public required string ConnectionString { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// It should be set when <see cref="DbType"/> is set to "SQLite".
    /// </remarks>
    public string? DatabaseName { get; set; }
}

