using System.Collections.Generic;
using System.Threading.Tasks;
using TicketManagement.BusinessLogic.Task1.Models;
using TicketManagement.DataAccess.Models;

namespace TicketManagement.BusinessLogic.Task1.Services.Interfaces
{
    /// <summary>
    /// Service working with <see cref="Layout"/>.
    /// </summary>
    public interface ILayoutService
    {
        Task<LayoutDto> GetByIdAsync(int id);
        Task<IEnumerable<LayoutDto>> GetLayoutsByVenue(int venueId);

        Task<bool> AddAsync(LayoutDto layout);

        Task<bool> UpdateAsync(LayoutDto layout);

        Task<bool> RemoveAsync(int layoutId);
    }
}