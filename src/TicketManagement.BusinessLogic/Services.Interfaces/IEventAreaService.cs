using System.Collections.Generic;
using System.Threading.Tasks;
using TicketManagement.BusinessLogic.Task1.Models;
using TicketManagement.DataAccess.Models;

namespace TicketManagement.BusinessLogic.Task1.Services.Interfaces
{
    /// <summary>
    /// Service working with <see cref="EventArea"/>.
    /// </summary>
    public interface IEventAreaService
    {
        Task<EventAreaDto> GetByIdAsync(int id);
        Task<IEnumerable<EventAreaDto>> GetEventAreasByEventId(int eventId);

        Task<bool> AddAsync(EventAreaDto eventArea);

        Task<bool> UpdateAsync(EventAreaDto eventArea);

        Task<bool> RemoveAsync(int id);
    }
}