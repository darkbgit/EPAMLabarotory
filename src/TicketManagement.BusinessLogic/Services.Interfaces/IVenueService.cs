using System.Threading.Tasks;
using TicketManagement.BusinessLogic.Task1.Models;
using TicketManagement.DataAccess.Models;

namespace TicketManagement.BusinessLogic.Task1.Services.Interfaces
{
    /// <summary>
    /// Service working with <see cref="Venue"/>.
    /// </summary>
    public interface IVenueService
    {
        Task<VenueDto> GetByIdAsync(int id);

        Task<bool> AddAsync(VenueDto venue);

        Task<bool> UpdateAsync(VenueDto venue);

        Task<bool> RemoveAsync(int venueId);
    }
}