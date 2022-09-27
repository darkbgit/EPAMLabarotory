using TicketManagement.DataAccess.EF.Core;
using TicketManagement.DataAccess.EF.Implementation.Repositories;
using TicketManagement.DataAccess.Public;
using TicketManagement.DataAccess.Public.Repositories;

namespace TicketManagement.DataAccess.EF.Implementation
{
    internal sealed class UnitOfWork : IUnitOfWork
    {
        private readonly TicketManagementContext _db;
        private readonly Lazy<IVenueRepository> _lazyVenueRepository;
        private readonly Lazy<ILayoutRepository> _lazyLayoutRepository;
        private readonly Lazy<IAreaRepository> _lazyAreaRepository;
        private readonly Lazy<ISeatRepository> _lazySeatRepository;
        private readonly Lazy<IEventRepository> _lazyEventRepository;
        private readonly Lazy<IEventAreaRepository> _lazyEventAreaRepository;
        private readonly Lazy<IEventSeatRepository> _lazyEventSeatRepository;
        private readonly Lazy<IOrderRepository> _lazyOrderRepository;

        public UnitOfWork(TicketManagementContext db)
        {
            _db = db;
            _lazyVenueRepository = new Lazy<IVenueRepository>(() => new VenueRepository(db));
            _lazyLayoutRepository = new Lazy<ILayoutRepository>(() => new LayoutRepository(db));
            _lazyAreaRepository = new Lazy<IAreaRepository>(() => new AreaRepository(db));
            _lazySeatRepository = new Lazy<ISeatRepository>(() => new SeatRepository(db));
            _lazyEventRepository = new Lazy<IEventRepository>(() => new EventRepository(db));
            _lazyEventAreaRepository = new Lazy<IEventAreaRepository>(() => new EventAreaRepository(db));
            _lazyEventSeatRepository = new Lazy<IEventSeatRepository>(() => new EventSeatRepository(db));
            _lazyOrderRepository = new Lazy<IOrderRepository>(() => new OrderRepository(db));
        }

        public IVenueRepository Venue => _lazyVenueRepository.Value;
        public ILayoutRepository Layout => _lazyLayoutRepository.Value;
        public IAreaRepository Area => _lazyAreaRepository.Value;
        public ISeatRepository Seat => _lazySeatRepository.Value;
        public IEventRepository Event => _lazyEventRepository.Value;
        public IEventAreaRepository EventArea => _lazyEventAreaRepository.Value;
        public IEventSeatRepository EventSeat => _lazyEventSeatRepository.Value;
        public IOrderRepository Order => _lazyOrderRepository.Value;

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _db.SaveChangesAsync(cancellationToken);
        }
    }
}