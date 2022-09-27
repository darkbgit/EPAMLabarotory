using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using TicketManagement.DataAccess.ADO.Data;
using TicketManagement.DataAccess.EF.Core.Entities;
using TicketManagement.DataAccess.Public.Repositories;

#pragma warning disable CA2100

namespace TicketManagement.DataAccess.ADO.Repositories
{
    internal class SeatRepository : Repository<Seat>, ISeatRepository
    {
        public SeatRepository(IConnectionFactory connectionFactory)
            : base(connectionFactory)
        {
        }

        private protected override string TableName => "[dbo].[Seat]";

        private protected override string ExistsCondition => "AreaId = @areaId AND Row = @row AND Number = @number";

        public async Task<IEnumerable<Seat>> GetSeatsByAreaIdAsync(int areaId)
        {
            string sqlExpression = $"SELECT {TableRowsSubstring} FROM {TableName} WHERE AreaId=@areaId";

            IList<Seat> result = new List<Seat>();

            using (var connection = await GetAndOpenConnectionAsync())
            {
                var command = new SqlCommand(sqlExpression, connection);

                command.Parameters.Add(new SqlParameter("areaId", areaId));

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (!reader.HasRows)
                    {
                        return null;
                    }

                    while (await reader.ReadAsync())
                    {
                        result.Add(Mapper.Mapper.Map<Seat>(reader));
                    }
                }
            }

            return result;
        }

        public async Task<int> AddRange(IEnumerable<Seat> seats)
        {
            var seatsList = seats.ToList();

            using (var connection = await GetAndOpenConnectionAsync())
            {
                var command = new SqlCommand();

                var dt = new DataTable();
                dt.Columns.Add("AreaId", typeof(int));
                dt.Columns.Add("Row", typeof(int));
                dt.Columns.Add("Number", typeof(int));

                for (int i = 0; i < seatsList.Count; i++)
                {
                    dt.Rows.Add(seatsList[i].AreaId, seatsList[i].Row, seatsList[i].Number);
                }

                command.Connection = connection;
                command.CommandText = "[dbo].[seat_AddRange]";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("seats", SqlDbType.Structured));
                command.Parameters["seats"].Value = dt;

                var result = (int)await command.ExecuteScalarAsync();
                return result;
            }
        }

        private protected override SqlParameter[] GetParamsForAddAndUpdate(Seat entity)
        {
            return new[]
            {
                new SqlParameter("@areaId", entity.AreaId),
                new SqlParameter("@row", entity.Row),
                new SqlParameter("@number", entity.Number),
            };
        }

        private protected override SqlParameter[] GetParamsForIsExist(Seat entity)
        {
            return new[]
            {
                new SqlParameter("@areaId", entity.AreaId),
                new SqlParameter("@row", entity.Row),
                new SqlParameter("@number", entity.Number),
            };
        }

        public IQueryable<Seat> GetSeatsByAreaId(int areaId)
        {
            throw new System.NotImplementedException();
        }
    }
}