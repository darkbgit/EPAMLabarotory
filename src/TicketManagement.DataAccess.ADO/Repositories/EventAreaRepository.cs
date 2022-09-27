using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using TicketManagement.Core.Public.DTOs.EventAreaDTOs;
using TicketManagement.DataAccess.ADO.Data;
using TicketManagement.DataAccess.EF.Core.Entities;
using TicketManagement.DataAccess.Public.Repositories;

#pragma warning disable CA2100
namespace TicketManagement.DataAccess.ADO.Repositories
{
    internal sealed class EventAreaRepository : Repository<EventArea>, IEventAreaRepository
    {
        public EventAreaRepository(IConnectionFactory connectionFactory)
            : base(connectionFactory)
        {
        }

        private protected override string TableName => "[dbo].[EventArea]";

        private protected override string ExistsCondition =>
            "EventId = @eventId AND (Description = @description OR (@coordX = @coordX AND CoordY = @coordY))";

        public void CreateEventArea(EventArea entity)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteEventArea(EventArea entity)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<EventArea>> GetAreasByEventIdAsync(int eventId)
        {
            string sqlExpression = $"SELECT {TableRowsSubstring} FROM {TableName} WHERE EventId=@eventId";

            IList<EventArea> result = new List<EventArea>();

            using (var connection = await GetAndOpenConnectionAsync())
            {
                var command = new SqlCommand(sqlExpression, connection);

                command.Parameters.Add(new SqlParameter("eventId", eventId));

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (!reader.HasRows)
                    {
                        return null;
                    }

                    while (await reader.ReadAsync())
                    {
                        result.Add(Mapper.Mapper.Map<EventArea>(reader));
                    }
                }
            }

            return result;
        }

        public IQueryable<EventArea> GetEventAreasByEventId(int eventId)
        {
            throw new System.NotImplementedException();
        }

        public IQueryable<EventAreaWithSeatsAndFreeSeatsCountDto> GetEventAreasWithSeatsAndFreeSeatsCountByEventId(int eventId)
        {
            throw new System.NotImplementedException();
        }

        public IQueryable<EventAreaWithSeatsNumberDto> GetEventAreasWithSeatsCountByEventId(int eventId)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> IsAnotherExistAsync(EventArea entity, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> IsAnotherExists(EventArea eventArea)
        {
            bool result;
            using (var connection = await GetAndOpenConnectionAsync())
            {
                var sql = $"SELECT CASE WHEN EXISTS (SELECT 1 FROM {TableName} WHERE Id != @id AND {ExistsCondition}) THEN CAST (1 AS BIT) ELSE CAST (0 AS BIT) END";

                var command = new SqlCommand(sql, connection);

                var parameters = GetParamsForIsExist(eventArea);

                command.Parameters.AddRange(parameters);
                command.Parameters.AddWithValue("@id", eventArea.Id);

                result = (bool)await command.ExecuteScalarAsync();
            }

            return result;
        }

        public Task<bool> IsExistAsync(EventArea entity, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateEventArea(EventArea entity)
        {
            throw new System.NotImplementedException();
        }

        private protected override SqlParameter[] GetParamsForAddAndUpdate(EventArea entity)
        {
            return new[]
            {
                new SqlParameter("@description", entity.Description),
                new SqlParameter("@eventId", entity.EventId),
                new SqlParameter("@coordX", entity.CoordX),
                new SqlParameter("@coordY", entity.CoordY),
                new SqlParameter("@price", entity.Price),
            };
        }

        private protected override SqlParameter[] GetParamsForIsExist(EventArea entity)
        {
            return new[]
            {
                new SqlParameter("@description", entity.Description),
                new SqlParameter("@eventId", entity.EventId),
                new SqlParameter("@coordX", entity.CoordX),
                new SqlParameter("@coordY", entity.CoordY),
            };
        }
    }
}