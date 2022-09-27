using TicketManagement.DataAccess.EF.Core;
using TicketManagement.IntegrationTests.Helpers;

namespace TicketManagement.IntegrationTests.WebApp
{
    internal static class Utilities
    {
        public static void InitializeDbForTests(TicketManagementContext db)
        {
            db.Venue.AddRange(TestData.Venues);
            db.Layout.AddRange(TestData.Layouts);
            db.Area.AddRange(TestData.Areas);
            db.Seat.AddRange(TestData.Seats);
            db.Event.AddRange(TestData.Events);
            db.EventArea.AddRange(TestData.EventAreas);
            db.EventSeat.AddRange(TestData.EventSeats);

            db.SaveChanges();
        }

        public static void ReinitializeDbForTests(TicketManagementContext db)
        {
            db.Venue.RemoveRange(db.Venue);
            InitializeDbForTests(db);
        }
    }
}