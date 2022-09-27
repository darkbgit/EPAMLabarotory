using System;
using System.Configuration;
using Microsoft.Data.SqlClient;

namespace TicketManagement.DataAccess.ADO.Data
{
    internal class SqlServerConnectionFactory : IConnectionFactory
    {
        private readonly string _connectionString;

        public SqlServerConnectionFactory(string connectionName)
        {
            if (connectionName == null)
            {
                throw new ArgumentNullException(nameof(connectionName));
            }

            var connectionString = ConfigurationManager.ConnectionStrings[connectionName];

            if (connectionString == null)
            {
                throw new ConfigurationErrorsException(
                    $"Failed to find connection string named {connectionName} in appsettings.json.");
            }

            _connectionString = connectionString.ConnectionString;
        }

        public SqlConnection Create() => new SqlConnection(_connectionString);
    }
}