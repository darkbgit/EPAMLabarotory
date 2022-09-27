using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using TicketManagement.DataAccess.ADO.Data;
using TicketManagement.DataAccess.EF.Core.Entities;
using TicketManagement.DataAccess.Public.Repositories;

#pragma warning disable CA2100

namespace TicketManagement.DataAccess.ADO.Repositories
{
    internal class EventSeatRepository : Repository<EventSeat>, IEventSeatRepository
    {
        public EventSeatRepository(IConnectionFactory connectionFactory)
            : base(connectionFactory)
        {
        }

        private protected override string TableName => "[dbo].[EventSeat]";

        private protected override string ExistsCondition => "EventAreaId = @eventAreaId AND Row = @row AND Number = @number";

        public async Task<IEnumerable<EventSeat>> GetSeatsByAreaIdAsync(int eventAreaId)
        {
            string sqlExpression = $"SELECT {TableRowsSubstring} FROM {TableName} WHERE EventAreaId=@eventAreaId";

            IList<EventSeat> result = new List<EventSeat>();

            using (var connection = await GetAndOpenConnectionAsync())
            {
                var command = new SqlCommand(sqlExpression, connection);

                command.Parameters.Add(new SqlParameter("eventAreaId", eventAreaId));

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (!reader.HasRows)
                    {
                        return null;
                    }

                    while (await reader.ReadAsync())
                    {
                        result.Add(Mapper.Mapper.Map<EventSeat>(reader));
                    }
                }
            }

            return result;
        }

        public async Task<bool> IsAnotherExists(EventSeat eventSeat)
        {
            bool result;
            using (var connection = await GetAndOpenConnectionAsync())
            {
                var sql = $"SELECT CASE WHEN EXISTS (SELECT 1 FROM {TableName} WHERE Id != @id AND {ExistsCondition}) THEN CAST (1 AS BIT) ELSE CAST (0 AS BIT) END";

                var command = new SqlCommand(sql, connection);

                var parameters = GetParamsForIsExist(eventSeat);

                command.Parameters.AddRange(parameters);
                command.Parameters.AddWithValue("@id", eventSeat.Id);

                result = (bool)await command.ExecuteScalarAsync();
            }

            return result;
        }

        public async Task<int> AddRange(IEnumerable<EventSeat> seats)
        {
            var seatsList = seats.ToList();

            using (var connection = await GetAndOpenConnectionAsync())
            {
                var command = new SqlCommand();

                var dt = new DataTable();
                dt.Columns.Add("EventAreaId", typeof(int));
                dt.Columns.Add("Row", typeof(int));
                dt.Columns.Add("Number", typeof(int));
                dt.Columns.Add("State", typeof(int));

                for (int i = 0; i < seatsList.Count; i++)
                {
                    dt.Rows.Add(seatsList[i].EventAreaId, seatsList[i].Row, seatsList[i].Number);
                }

                command.Connection = connection;
                command.CommandText = "[dbo].[eventSeat_AddRange]";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("seats", SqlDbType.Structured));
                command.Parameters["seats"].Value = dt;

                var result = (int)await command.ExecuteScalarAsync();
                return result;
            }
        }

        private protected override SqlParameter[] GetParamsForAddAndUpdate(EventSeat entity)
        {
            return new[]
            {
                new SqlParameter("@eventAreaId", entity.EventAreaId),
                new SqlParameter("@row", entity.Row),
                new SqlParameter("@number", entity.Number),
                new SqlParameter("@state", entity.State),
            };
        }

        private protected override SqlParameter[] GetParamsForIsExist(EventSeat entity)
        {
            return new[]
            {
                new SqlParameter("@eventAreaId", entity.EventAreaId),
                new SqlParameter("@row", entity.Row),
                new SqlParameter("@number", entity.Number),
            };
        }

        public IQueryable<EventSeat> GetEventSeatsByEventAreaId(int eventAreaId)
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateStateAsync(int eventSeatId, int newState, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> IsExistAsync(EventSeat entity, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> IsAnotherExistAsync(EventSeat entity, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> IsExistWithStateAsync(int eventSeatId, int seatState, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public void AddEventSeatsRange(IEnumerable<EventSeat> eventSeats)
        {
            throw new System.NotImplementedException();
        }

        public void CreateEventSeat(EventSeat entity)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateEventSeat(EventSeat entity)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteEventSeat(EventSeat entity)
        {
            throw new System.NotImplementedException();
        }
    }
}