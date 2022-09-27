using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using TicketManagement.DataAccess.ADO.Data;
using TicketManagement.DataAccess.ADO.Extensions;
using TicketManagement.DataAccess.EF.Core.Entities;
using TicketManagement.DataAccess.Public.Repositories;

#pragma warning disable CA2100

namespace TicketManagement.DataAccess.ADO.Repositories
{
    internal abstract class Repository<T> : IRepository<T>
        where T : class, IBaseEntity, new()
    {
        private readonly IConnectionFactory _connectionFactory;

        private protected Repository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
            InitializeSubstrings();
        }

        private protected abstract string TableName { get; }
        private protected abstract string ExistsCondition { get; }
        private protected string TableRowsSubstring { get; private set; }
        private string AddSubstring { get; set; }
        private string UpdateSubstring { get; set; }

        private protected async Task<SqlConnection> GetAndOpenConnectionAsync()
        {
            var connection = _connectionFactory.Create();
            await connection.OpenAsync();

            return connection;
        }

        public virtual async Task<T> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            await using (var connection = await GetAndOpenConnectionAsync())
            {
                var sql = $"SELECT {TableRowsSubstring} FROM {TableName} WHERE Id=@id";

                var command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@id", id);

                await using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                {
                    if (!reader.HasRows)
                    {
                        return null;
                    }

                    if (await reader.ReadAsync(cancellationToken))
                    {
                        return Mapper.Mapper.Map<T>(reader);
                    }
                }
            }

            return null;
        }

        public IQueryable<T> FindAll()
        {
            throw new NotImplementedException();
        }

        public virtual async Task<int> AddAsync(T entity)
        {
            await using (var connection = await GetAndOpenConnectionAsync())
            {
                var sql =
                    $"IF NOT EXISTS(SELECT 1 FROM {TableName} WHERE {ExistsCondition}) " +
                    "BEGIN " +
                    $"INSERT INTO {TableName} {AddSubstring} " +
                    "SELECT CAST(SCOPE_IDENTITY() AS int) RETURN " +
                    "END " +
                    "SELECT -1";

                var command = new SqlCommand(sql, connection);

                var parameters = GetParamsForAddAndUpdate(entity);

                command.Parameters.AddRange(parameters);

                var result = (int)await command.ExecuteScalarAsync();

                return result;
            }
        }

        public virtual async Task<int> UpdateAsync(T entity)
        {
            await using (var connection = await GetAndOpenConnectionAsync())
            {
                var sql = $"IF EXISTS (SELECT 1 FROM {TableName} WHERE Id = @id) " +
                          "BEGIN " +
                          $"UPDATE {TableName} SET {UpdateSubstring} " +
                          "SELECT CAST(@@ROWCOUNT AS int) RETURN " +
                          "END " +
                          "SELECT - 1";

                var command = new SqlCommand(sql, connection);

                var parameters = GetParamsForAddAndUpdate(entity);

                command.Parameters.AddRange(parameters);

                command.Parameters.AddWithValue("@Id", entity.Id);

                var result = (int)await command.ExecuteScalarAsync();

                return result;
            }
        }

        public virtual async Task<int> RemoveAsync(int id)
        {
            await using (var connection = await GetAndOpenConnectionAsync())
            {
                var sql = $"DELETE FROM {TableName} WHERE Id = @id SELECT CAST(@@ROWCOUNT AS int)";

                var command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@id", id);

                var result = (int)await command.ExecuteScalarAsync();

                return result;
            }
        }

        public virtual async Task<bool> IsExists(T entity)
        {
            bool result;
            using (var connection = await GetAndOpenConnectionAsync())
            {
                var sql = $"SELECT CASE WHEN EXISTS (SELECT 1 FROM {TableName} WHERE {ExistsCondition}) THEN CAST (1 AS BIT) ELSE CAST (0 AS BIT) END";

                var command = new SqlCommand(sql, connection);

                var parameters = GetParamsForIsExist(entity);

                command.Parameters.AddRange(parameters);

                result = (bool)await command.ExecuteScalarAsync();
            }

            return result;
        }

        private protected abstract SqlParameter[] GetParamsForAddAndUpdate(T entity);

        private protected abstract SqlParameter[] GetParamsForIsExist(T entity);

        private void InitializeSubstrings()
        {
            var props = typeof(T).GetProperties().Select(p => p.Name).ToList();

            TableRowsSubstring = string.Join(", ", props);

            var propsWithoutId = props.Where(p => p != "Id").ToList();

            AddSubstring = $"({string.Join(", ", propsWithoutId)}) VALUES ({string.Join(", ", propsWithoutId.Select(p => string.Concat("@", p.FirstCharToLowerCase())))})";

            UpdateSubstring = string.Join(", ", propsWithoutId.Select(p => string.Concat(p, " = @", p.FirstCharToLowerCase())));
        }

        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }
    }
}