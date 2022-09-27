using System.Collections.Generic;
using System.Threading.Tasks;
using TicketManagement.BusinessLogic.Task1.Models;
using TicketManagement.DataAccess.Models;

namespace TicketManagement.BusinessLogic.Task1.Services.Interfaces
{
    /// <summary>
    /// Service working with <see cref="EventSeat"/>.
    /// </summary>
    public interface IEventSeatService
    {
        Task<EventSeatDto> GetByIdAsync(int id);
        Task<IEnumerable<EventSeatDto>> GetEventSeatsByEventAreaId(int eventAreaId);

        Task<bool> AddAsync(EventSeatDto eventSeat);

        Task<bool> UpdateAsync(EventSeatDto eventSeat);

        Task<bool> RemoveAsync(int id);
    }
}