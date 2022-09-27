using System;
using System.IO;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.SqlServer.Dac;
using NUnit.Framework;

namespace TicketManagement.IntegrationTests.Helpers;

internal class TestDatabase : IDisposable
{
    private string _databaseName;
    private string _connectionString;

    private bool _initialized;
    private bool _disposedValue;

    public void Initialise()
    {
        var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        _databaseName = $"IntegrationTestDB_{Guid.NewGuid().ToString("N").ToUpper()}";

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        var dacpacRelativePath = configuration.GetConnectionString("DacpacPath");

        var instance = new DacServices(connectionString);

        var dacpacPath = Path.GetFullPath(
            Path.Combine(
                TestContext.CurrentContext.TestDirectory,
                dacpacRelativePath));

        using (var dacpac = DacPackage.Load(dacpacPath))
        {
            instance.Deploy(dacpac, _databaseName, upgradeExisting: true);
        }

        _connectionString = string.Concat(connectionString, $"Initial Catalog={_databaseName}");
        _initialized = true;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "Used during testing")]
    private void Drop()
    {
        if (!_initialized)
        {
            return;
        }

        using (var connection = GetSqlConnection())
        {
            connection.Open();
            connection.ChangeDatabase("master");

            using (var command =
                   new SqlCommand($"ALTER DATABASE [{_databaseName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE [{_databaseName}]", connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "Used during testing")]
    public void RunSetUpScript(string sql)
    {
        RunCleanUpScript();

        using (var connection = GetSqlConnection())
        {
            connection.Open();

            var command = new SqlCommand(sql, connection);

            command.ExecuteNonQuery();
        }
    }

    public void RunCleanUpScript()
    {
        using (var connection = GetSqlConnection())
        {
            var sql =
                "DELETE FROM dbo.Venue;";

            connection.Open();

            var command = new SqlCommand(sql, connection);

            command.ExecuteNonQuery();
        }
    }

    public SqlConnection GetSqlConnection()
    {
        if (_initialized)
        {
            return new SqlConnection(_connectionString);
        }

        throw new InvalidOperationException("Instance of TestDatabase don't initialized.");
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                Drop();
            }

            _disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}