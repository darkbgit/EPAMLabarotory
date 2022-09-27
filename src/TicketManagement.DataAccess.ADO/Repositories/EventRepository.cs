using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using TicketManagement.Core.Public.DTOs.EventDTOs;
using TicketManagement.DataAccess.ADO.Data;
using TicketManagement.DataAccess.EF.Core.Entities;
using TicketManagement.DataAccess.Public.Repositories;

#pragma warning disable CA2100
namespace TicketManagement.DataAccess.ADO.Repositories
{
    internal class EventRepository : Repository<Event>, IEventRepository
    {
        public EventRepository(IConnectionFactory connectionFactory)
            : base(connectionFactory)
        {
        }

        private protected override string TableName => "[dbo].[Event]";

        private protected override string ExistsCondition =>
            "LayoutId = @layoutId AND (Description = @description OR Name = @name OR (StartDate < @startDate AND EndDate > @startDate) OR (StartDate < @endDate AND EndDate > @endDate))";

        public override async Task<int> AddAsync(Event entity)
        {
            await using (var connection = await GetAndOpenConnectionAsync())
            {
                const string storedProcedure = "event_Add";

                var command = new SqlCommand(storedProcedure, connection)
                {
                    CommandType = CommandType.StoredProcedure,
                };

                var parameters = GetParamsForAddAndUpdate(entity);

                command.Parameters.AddRange(parameters);

                var result = (int)await command.ExecuteScalarAsync();

                return result;
            }
        }

        public override async Task<int> UpdateAsync(Event entity)
        {
            await using (var connection = await GetAndOpenConnectionAsync())
            {
                const string storedProcedure = "event_Update";

                var command = new SqlCommand(storedProcedure, connection)
                {
                    CommandType = CommandType.StoredProcedure,
                };

                var parameters = GetParamsForAddAndUpdate(entity);

                command.Parameters.AddRange(parameters);

                command.Parameters.AddWithValue("@Id", entity.Id);

                var result = (int)await command.ExecuteScalarAsync();

                return result;
            }
        }

        public override async Task<int> RemoveAsync(int id)
        {
            await using (var connection = await GetAndOpenConnectionAsync())
            {
                const string storedProcedure = "event_Remove";

                var command = new SqlCommand(storedProcedure, connection)
                {
                    CommandType = CommandType.StoredProcedure,
                };

                command.Parameters.AddWithValue("@id", id);

                var result = (int)await command.ExecuteScalarAsync();

                return result;
            }
        }

        public async Task<IEnumerable<Event>> GetEventsByLayoutId(int layoutId)
        {
            string sqlExpression = $"SELECT {TableRowsSubstring} FROM {TableName} WHERE layoutId=@layoutId";

            IList<Event> result = new List<Event>();

            using (var connection = await GetAndOpenConnectionAsync())
            {
                var command = new SqlCommand(sqlExpression, connection);

                command.Parameters.Add(new SqlParameter("layoutId", layoutId));

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (!reader.HasRows)
                    {
                        return null;
                    }

                    while (await reader.ReadAsync())
                    {
                        result.Add(Mapper.Mapper.Map<Event>(reader));
                    }
                }
            }

            return result;
        }

        public async Task<bool> IsAnotherExists(Event eventEntity)
        {
            bool result;
            using (var connection = await GetAndOpenConnectionAsync())
            {
                var sql = $"SELECT CASE WHEN EXISTS (SELECT 1 FROM {TableName} WHERE Id != @id AND {ExistsCondition}) THEN CAST (1 AS BIT) ELSE CAST (0 AS BIT) END";

                var command = new SqlCommand(sql, connection);

                var parameters = GetParamsForIsExist(eventEntity);

                command.Parameters.AddRange(parameters);
                command.Parameters.AddWithValue("@id", eventEntity.Id);

                result = (bool)await command.ExecuteScalarAsync();
            }

            return result;
        }

        private protected override SqlParameter[] GetParamsForAddAndUpdate(Event entity)
        {
            return new[]
            {
                new SqlParameter("@description", entity.Description),
                new SqlParameter("@name", entity.Name),
                new SqlParameter("@layoutId", entity.LayoutId),
                new SqlParameter("@startDate", entity.StartDate),
                new SqlParameter("@endDate", entity.EndDate),
            };
        }

        private protected override SqlParameter[] GetParamsForIsExist(Event entity)
        {
            return new[]
            {
                new SqlParameter("@description", entity.Description),
                new SqlParameter("@name", entity.Name),
                new SqlParameter("@layoutId", entity.LayoutId),
                new SqlParameter("@startDate", entity.StartDate),
                new SqlParameter("@endDate", entity.EndDate),
            };
        }

        public Task<EventWithVenueIdDto> GetEventWithVenueIdByIdAsync(int eventId, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> IsExistAsync(Event entity, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> IsExistForUpdateAsync(Event entity, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public int CreateEvent(Event entity)
        {
            throw new System.NotImplementedException();
        }

        public int UpdateEvent(Event entity)
        {
            throw new System.NotImplementedException();
        }

        public int DeleteEvent(int eventId)
        {
            throw new System.NotImplementedException();
        }

        public IQueryable<EventForListDto> GetEventsForMainPage()
        {
            throw new System.NotImplementedException();
        }

        public IQueryable<EventForEditListDto> GetEventsForEditList()
        {
            throw new System.NotImplementedException();
        }

        public IQueryable<EventWithVenueIdDto> GetEventsWithVenueIdById()
        {
            throw new System.NotImplementedException();
        }

        public IQueryable<EventForDetailsDto> GetEventsWithDetailsById()
        {
            throw new System.NotImplementedException();
        }

        public IQueryable<EventInfoForEventAreaListDto> GetEventsForEventAreaList()
        {
            throw new System.NotImplementedException();
        }

        public IQueryable<EventWithLayoutsDto> GetEventsWithLayoutsAndVenueTimeZoneById()
        {
            throw new System.NotImplementedException();
        }

        public Task<int> EventsForEditListCountAsync(string searchString, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> EventsForManePageCountAsync(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}