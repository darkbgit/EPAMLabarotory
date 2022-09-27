using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TicketManagement.Core.EventManagement.Services.Interfaces;
using TicketManagement.Core.Public.DTOs.AreaDTOs;
using TicketManagement.DataAccess.Public;

namespace TicketManagement.Core.EventManagement.Services
{
    internal sealed class AreaService : IAreaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AreaService(IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AreaDto>> GetAreasByLayoutIdAsync(int layoutId, CancellationToken cancellationToken = default)
        {
            var result = await _unitOfWork.Area
                .GetAreasByLayoutId(layoutId)
                .Select(area => _mapper.Map<AreaDto>(area))
                .ToListAsync(cancellationToken);

            return result;
        }

        public async Task<IEnumerable<AreaWithSeatsNumberDto>> GetAreasWithSeatsNumberByLayoutIdAsync(int layoutId, CancellationToken cancellationToken = default)
        {
            var result = await _unitOfWork.Area
                .GetAreasWithSeatsNumberByLayoutId(layoutId)
                .ToListAsync(cancellationToken);

            return result;
        }
    }
}