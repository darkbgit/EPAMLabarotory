using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using TicketManagement.DataAccess.ADO.Data;
using TicketManagement.DataAccess.EF.Core.Entities;
using TicketManagement.DataAccess.Public.Repositories;

#pragma warning disable CA2100

namespace TicketManagement.DataAccess.ADO.Repositories
{
    internal sealed class VenueRepository : Repository<Venue>, IVenueRepository
    {
        public VenueRepository(IConnectionFactory connectionFactory)
            : base(connectionFactory)
        {
        }

        private protected override string TableName => "[dbo].[Venue]";

        private protected override string ExistsCondition => "Description = @description";

        public Task<Venue> GetByDescription()
        {
            throw new NotImplementedException();
        }

        public async Task<Venue> GetVenueByNameAsync(string name)
        {
            string sqlExpression = $"SELECT {TableRowsSubstring} FROM {TableName} WHERE Description=@Description";

            using (var connection = await GetAndOpenConnectionAsync())
            {
                var command = new SqlCommand(sqlExpression, connection);
                command.Parameters.Add(new SqlParameter("@Description", name));

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (!reader.HasRows)
                    {
                        return null;
                    }

                    if (await reader.ReadAsync())
                    {
                        return Mapper.Mapper.Map<Venue>(reader);
                    }

                    throw new DataException();
                }
            }
        }

        private protected override SqlParameter[] GetParamsForAddAndUpdate(Venue entity)
        {
            return new[]
            {
                new SqlParameter("@description", entity.Description),
                new SqlParameter("@address", entity.Address),
                new SqlParameter("@phone", SqlDbType.Text)
                {
                    Value = (object)entity.Phone ?? DBNull.Value,
                },
            };
        }

        private protected override SqlParameter[] GetParamsForIsExist(Venue entity)
        {
            return new[]
            {
                new SqlParameter("@description", entity.Description),
            };
        }

        public void CreateVenue(Venue venue)
        {
            throw new NotImplementedException();
        }

        public void UpdateVenue(Venue venue)
        {
            throw new NotImplementedException();
        }

        public void DeleteVenue(Venue venue)
        {
            throw new NotImplementedException();
        }
    }
}