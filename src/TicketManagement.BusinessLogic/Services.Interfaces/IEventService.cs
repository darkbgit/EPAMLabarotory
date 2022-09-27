using System.Collections.Generic;
using System.Threading.Tasks;
using TicketManagement.BusinessLogic.Task1.Models;
using TicketManagement.DataAccess.Models;

namespace TicketManagement.BusinessLogic.Task1.Services.Interfaces
{
    /// <summary>
    /// Service working with <see cref="Event"/>.
    /// </summary>
    public interface IEventService
    {
        Task<EventDto> GetByIdAsync(int id);
        Task<IEnumerable<EventDto>> GetEventsByLayout(int layoutId);
        Task<bool> AddAsync(EventDto eventDto);

        Task<bool> UpdateAsync(EventDto eventDto);

        Task<bool> RemoveAsync(int eventId);
    }
}