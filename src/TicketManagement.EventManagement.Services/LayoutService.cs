using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TicketManagement.Core.EventManagement.Services.Interfaces;
using TicketManagement.Core.Public.DTOs.LayoutDTOs;
using TicketManagement.DataAccess.Public;

namespace TicketManagement.Core.EventManagement.Services
{
    internal sealed class LayoutService : ILayoutService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LayoutService(IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<LayoutDto>> GetLayoutsByVenueIdAsync(int venueId, CancellationToken cancellationToken = default)
        {
            var result = await _unitOfWork.Layout
                .GetLayoutsByVenueId(venueId)
                .Select(layout => _mapper.Map<LayoutDto>(layout))
                .ToListAsync(cancellationToken);

            return result;
        }
    }
}
