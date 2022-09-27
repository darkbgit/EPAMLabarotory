using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using TicketManagement.DataAccess.ADO.Data;
using TicketManagement.DataAccess.EF.Core.Entities;
using TicketManagement.DataAccess.Public.Repositories;

#pragma warning disable CA2100

namespace TicketManagement.DataAccess.ADO.Repositories
{
    internal class LayoutRepository : Repository<Layout>, ILayoutRepository
    {
        public LayoutRepository(IConnectionFactory connectionFactory)
            : base(connectionFactory)
        {
        }

        private protected override string TableName => "[dbo].[Layout]";

        private protected override string ExistsCondition => "Description = @description AND VenueId = @venueId";

        public IQueryable<Layout> GetLayoutsByVenueId(int venueId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<Layout>> GetLayoutsByVenueIdAsync(int venueId)
        {
            string sqlExpression = $"SELECT {TableRowsSubstring} FROM {TableName} WHERE VenueId=@VenueId";

            IList<Layout> result = new List<Layout>();

            using (var connection = await GetAndOpenConnectionAsync())
            {
                var command = new SqlCommand(sqlExpression, connection);

                command.Parameters.Add(new SqlParameter("VenueId", venueId));

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (!reader.HasRows)
                    {
                        return null;
                    }

                    while (await reader.ReadAsync())
                    {
                        result.Add(Mapper.Mapper.Map<Layout>(reader));
                    }
                }
            }

            return result;
        }

        private protected override SqlParameter[] GetParamsForAddAndUpdate(Layout entity)
        {
            return new[]
            {
                new SqlParameter("@description", entity.Description),
                new SqlParameter("@venueId", entity.VenueId),
            };
        }

        private protected override SqlParameter[] GetParamsForIsExist(Layout entity)
        {
            return new[]
            {
                new SqlParameter("@description", entity.Description),
                new SqlParameter("@venueId", entity.VenueId),
            };
        }
    }
}