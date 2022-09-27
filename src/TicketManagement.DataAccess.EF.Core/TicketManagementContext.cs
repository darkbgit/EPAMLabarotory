using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TicketManagement.DataAccess.EF.Core.Entities;

namespace TicketManagement.DataAccess.EF.Core
{
    public class TicketManagementContext : IdentityDbContext<User, Role, Guid>
    {
        public TicketManagementContext(DbContextOptions<TicketManagementContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Venue> Venue { get; set; }
        public DbSet<Layout> Layout { get; set; }
        public virtual DbSet<Area> Area { get; set; }
        public DbSet<Seat> Seat { get; set; }
        public DbSet<Event> Event { get; set; }
        public DbSet<EventArea> EventArea { get; set; }
        public DbSet<EventSeat> EventSeat { get; set; }
        public DbSet<Order> Order { get; set; }
    }
}