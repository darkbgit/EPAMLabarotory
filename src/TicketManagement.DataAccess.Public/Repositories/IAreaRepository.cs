using System.Linq;
using TicketManagement.Core.Public.DTOs.AreaDTOs;
using TicketManagement.DataAccess.EF.Core.Entities;

namespace TicketManagement.DataAccess.Public.Repositories
{
    public interface IAreaRepository : IRepository<Area>
    {
        IQueryable<Area> GetAreasByLayoutId(int layoutId);

        IQueryable<AreaWithSeatsNumberDto> GetAreasWithSeatsNumberByLayoutId(int layoutId);
    }
}