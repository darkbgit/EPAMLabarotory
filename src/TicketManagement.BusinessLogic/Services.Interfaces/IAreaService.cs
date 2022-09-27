using System.Collections.Generic;
using System.Threading.Tasks;
using TicketManagement.BusinessLogic.Task1.Models;
using TicketManagement.DataAccess.Models;

namespace TicketManagement.BusinessLogic.Task1.Services.Interfaces
{
    /// <summary>
    /// Service working with <see cref="Area"/>.
    /// </summary>
    public interface IAreaService
    {
        Task<AreaDto> GetByIdAsync(int id);
        Task<IEnumerable<AreaDto>> GetAreasByLayoutId(int layoutId);

        Task<bool> AddAsync(AreaDto area);

        Task<bool> UpdateAsync(AreaDto area);

        Task<bool> RemoveAsync(int areaId);
    }
}