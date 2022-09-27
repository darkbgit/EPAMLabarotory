using System.Collections.Generic;
using System.Threading.Tasks;
using TicketManagement.BusinessLogic.Task1.Models;
using TicketManagement.DataAccess.Models;

namespace TicketManagement.BusinessLogic.Task1.Services.Interfaces
{
    /// <summary>
    /// Service working with <see cref="Seat"/>.
    /// </summary>
    public interface ISeatService
    {
        Task<SeatDto> GetByIdAsync(int id);
        Task<IEnumerable<SeatDto>> GetSeatsByAreaId(int areaId);

        Task<bool> AddAsync(SeatDto seat);

        Task<bool> UpdateAsync(SeatDto seat);

        Task<bool> RemoveAsync(int seatId);
    }
}