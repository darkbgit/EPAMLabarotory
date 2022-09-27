using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using TicketManagement.Core.Public.DTOs.AreaDTOs;
using TicketManagement.DataAccess.ADO.Data;
using TicketManagement.DataAccess.EF.Core.Entities;
using TicketManagement.DataAccess.Public.Repositories;

#pragma warning disable CA2100

namespace TicketManagement.DataAccess.ADO.Repositories
{
    internal sealed class AreaRepository : Repository<Area>, IAreaRepository
    {
        public AreaRepository(IConnectionFactory connectionFactory)
            : base(connectionFactory)
        {
        }

        private protected override string TableName => "[dbo].[Area]";

        private protected override string ExistsCondition =>
            "LayoutId = @layoutId AND (Description = @description OR (@coordX = @coordX AND CoordY = @coordY))";

        public IQueryable<Area> GetAreasByLayoutId(int layoutId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<Area>> GetAreasByLayoutIdAsync(int layoutId)
        {
            string sqlExpression = $"SELECT {TableRowsSubstring} FROM {TableName} WHERE LayoutId=@LayoutId";

            IList<Area> result = new List<Area>();

            using (var connection = await GetAndOpenConnectionAsync())
            {
                var command = new SqlCommand(sqlExpression, connection);

                command.Parameters.Add(new SqlParameter("LayoutId", layoutId));

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (!reader.HasRows)
                    {
                        return null;
                    }

                    while (await reader.ReadAsync())
                    {
                        result.Add(Mapper.Mapper.Map<Area>(reader));
                    }
                }
            }

            return result;
        }

        public IQueryable<AreaWithSeatsNumberDto> GetAreasWithSeatsNumberByLayoutId(int layoutId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> IsAnotherExists(Area area)
        {
            bool result;
            using (var connection = await GetAndOpenConnectionAsync())
            {
                var sql = $"SELECT CASE WHEN EXISTS (SELECT 1 FROM {TableName} WHERE Id != @id AND {ExistsCondition}) THEN CAST (1 AS BIT) ELSE CAST (0 AS BIT) END";

                var command = new SqlCommand(sql, connection);

                var parameters = GetParamsForIsExist(area);

                command.Parameters.AddRange(parameters);
                command.Parameters.AddWithValue("@id", area.Id);

                result = (bool)await command.ExecuteScalarAsync();
            }

            return result;
        }

        private protected override SqlParameter[] GetParamsForAddAndUpdate(Area entity)
        {
            return new[]
            {
                new SqlParameter("@description", entity.Description),
                new SqlParameter("@layoutId", entity.LayoutId),
                new SqlParameter("@coordX", entity.CoordX),
                new SqlParameter("@coordY", entity.CoordY),
            };
        }

        private protected override SqlParameter[] GetParamsForIsExist(Area entity)
        {
            return new[]
            {
                new SqlParameter("@layoutId", entity.LayoutId),
                new SqlParameter("@description", entity.Description),
                new SqlParameter("@coordX", entity.CoordX),
                new SqlParameter("@coordY", entity.CoordY),
            };
        }

        IQueryable<AreaWithSeatsNumberDto> IAreaRepository.GetAreasWithSeatsNumberByLayoutId(int layoutId)
        {
            throw new System.NotImplementedException();
        }
    }
}