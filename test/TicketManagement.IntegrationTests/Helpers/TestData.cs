using System;
using System.Linq;
using System.Text;
using TicketManagement.DataAccess.EF.Core.Entities;

namespace TicketManagement.IntegrationTests.Helpers;

internal static class TestData
{
    public static Venue[] Venues { get; } =
    {
        new Venue { Id = 1, Description = "FirstVenue", Address = "FirstVenueAddress", Phone = "FirstVenuePhone", TimeZoneId = "" },
    };

    public static Layout[] Layouts { get; } =
    {
        new Layout { Id = 1, VenueId = 1, Description = "First Layout" },
        new Layout { Id = 2, VenueId = 1, Description = "Second Layout" },
    };

    public static Area[] Areas { get; } =
    {
        new Area { Id = 1, Description = "FirstAreaInFirstLayout", CoordX = 0, CoordY = 0, LayoutId = 1 },
        new Area { Id = 2, Description = "SecondAreaInFirstLayout", CoordX = 1, CoordY = 1, LayoutId = 1 },
        new Area { Id = 3, Description = "FirstAreaInSecondLayout", CoordX = 0, CoordY = 0, LayoutId = 2 },
        new Area { Id = 4, Description = "SecondAreaInSecondLayout", CoordX = 1, CoordY = 1, LayoutId = 2 },
    };

    public static Seat[] Seats { get; } =
    {
        new Seat { Id = 1, Row = 1, Number = 1, AreaId = 1 },
        new Seat { Id = 2, Row = 1, Number = 2, AreaId = 1 },
        new Seat { Id = 3, Row = 1, Number = 3, AreaId = 1 },
        new Seat { Id = 4, Row = 1, Number = 4, AreaId = 1 },
        new Seat { Id = 5, Row = 1, Number = 5, AreaId = 1 },
        new Seat { Id = 6, Row = 1, Number = 1, AreaId = 2 },
        new Seat { Id = 7, Row = 1, Number = 2, AreaId = 2 },
        new Seat { Id = 8, Row = 1, Number = 3, AreaId = 2 },
        new Seat { Id = 9, Row = 1, Number = 4, AreaId = 2 },
        new Seat { Id = 10, Row = 1, Number = 5, AreaId = 2 },
        new Seat { Id = 11, Row = 1, Number = 1, AreaId = 3 },
        new Seat { Id = 12, Row = 1, Number = 2, AreaId = 3 },
        new Seat { Id = 13, Row = 1, Number = 3, AreaId = 3 },
        new Seat { Id = 14, Row = 1, Number = 4, AreaId = 3 },
        new Seat { Id = 15, Row = 1, Number = 5, AreaId = 3 },
        new Seat { Id = 16, Row = 1, Number = 1, AreaId = 4 },
        new Seat { Id = 17, Row = 1, Number = 2, AreaId = 4 },
        new Seat { Id = 18, Row = 1, Number = 3, AreaId = 4 },
        new Seat { Id = 19, Row = 1, Number = 4, AreaId = 4 },
        new Seat { Id = 20, Row = 1, Number = 5, AreaId = 4 },
    };

    public static Event[] Events { get; } =
    {
        new Event { Id = 1, LayoutId = 1, Name = "Name1", Description = "First Event", StartDate = DateTime.Now.AddDays(1), EndDate = DateTime.Now.AddDays(2), ImageUrl = "" },
        new Event { Id = 2, LayoutId = 1, Name = "Name1", Description = "Second Event", StartDate = DateTime.Now.AddDays(3), EndDate = DateTime.Now.AddDays(4), ImageUrl = "" },
    };

    public static EventArea[] EventAreas { get; } =
    {
        new EventArea { Id = 1, Description = "FirstEventAreaInFirstEvent", CoordX = 0, CoordY = 0, EventId = 1, Price = 10 },
        new EventArea { Id = 2, Description = "SecondEventAreaInFirstEvent", CoordX = 1, CoordY = 1, EventId = 1, Price = 15 },
        new EventArea { Id = 3, Description = "FirstEventAreaInSecondEvent", CoordX = 0, CoordY = 0, EventId = 2, Price = 20 },
        new EventArea { Id = 4, Description = "SecondEventAreaInSecondEvent", CoordX = 1, CoordY = 1, EventId = 2, Price = 25 },
    };

    public static EventSeat[] EventSeats { get; } =
    {
        new EventSeat { Id = 1, Row = 1, Number = 1, State = 0, EventAreaId = 1 },
        new EventSeat { Id = 2, Row = 1, Number = 2, State = 0, EventAreaId = 1 },
        new EventSeat { Id = 3, Row = 1, Number = 3, State = 0, EventAreaId = 1 },
        new EventSeat { Id = 4, Row = 1, Number = 4, State = 0, EventAreaId = 1 },
        new EventSeat { Id = 5, Row = 1, Number = 5, State = 0, EventAreaId = 1 },
        new EventSeat { Id = 6, Row = 1, Number = 1, State = 0, EventAreaId = 1 },
        new EventSeat { Id = 7, Row = 1, Number = 2, State = 0, EventAreaId = 1 },
        new EventSeat { Id = 8, Row = 1, Number = 3, State = 0, EventAreaId = 1 },
        new EventSeat { Id = 9, Row = 1, Number = 4, State = 0, EventAreaId = 1 },
        new EventSeat { Id = 10, Row = 1, Number = 5, State = 0, EventAreaId = 1 },
        new EventSeat { Id = 11, Row = 1, Number = 1, State = 0, EventAreaId = 1 },
        new EventSeat { Id = 12, Row = 1, Number = 2, State = 0, EventAreaId = 1 },
        new EventSeat { Id = 13, Row = 1, Number = 3, State = 0, EventAreaId = 1 },
        new EventSeat { Id = 14, Row = 1, Number = 4, State = 0, EventAreaId = 1 },
        new EventSeat { Id = 15, Row = 1, Number = 5, State = 0, EventAreaId = 1 },
        new EventSeat { Id = 16, Row = 1, Number = 1, State = 0, EventAreaId = 1 },
        new EventSeat { Id = 17, Row = 1, Number = 2, State = 0, EventAreaId = 1 },
        new EventSeat { Id = 18, Row = 1, Number = 3, State = 0, EventAreaId = 1 },
        new EventSeat { Id = 19, Row = 1, Number = 4, State = 0, EventAreaId = 1 },
        new EventSeat { Id = 20, Row = 1, Number = 5, State = 0, EventAreaId = 1 },
    };

    public static string GetInitializeSqlString()
    {
        var builder = new StringBuilder();

        builder.Append("SET IDENTITY_INSERT dbo.Venue ON; ");
        builder.Append("INSERT INTO dbo.Venue (Id, Description, Address, Phone, TimeZoneId) ");
        builder.Append("VALUES ");

        builder.Append(string.Join(", ",
            Venues.Select(v => $"({v.Id}, '{v.Description}', '{v.Address}', '{v.Phone}', '{v.TimeZoneId}')")));

        builder.Append("SET IDENTITY_INSERT dbo.Venue OFF;");

        builder.Append("SET IDENTITY_INSERT dbo.Layout ON; ");
        builder.Append("INSERT INTO dbo.Layout (Id, VenueId, Description) ");
        builder.Append("VALUES ");

        builder.Append(string.Join(", ",
            Layouts.Select(l => $"({l.Id}, {l.VenueId}, '{l.Description}')")));

        builder.Append("SET IDENTITY_INSERT dbo.Layout OFF;");

        builder.Append("SET IDENTITY_INSERT dbo.Area ON; ");
        builder.Append("INSERT INTO dbo.Area (Id, LayoutId, Description, CoordX, CoordY) ");
        builder.Append("VALUES ");

        builder.Append(string.Join(", ",
            Areas.Select(a => $"({a.Id}, {a.LayoutId}, '{a.Description}', {a.CoordX}, {a.CoordY})")));

        builder.Append("SET IDENTITY_INSERT dbo.Area OFF;");

        builder.Append("SET IDENTITY_INSERT dbo.Seat ON; ");
        builder.Append("INSERT INTO dbo.Seat (Id, AreaId, Row, Number) ");
        builder.Append("VALUES ");

        builder.Append(string.Join(", ",
            Seats.Select(s => $"({s.Id}, {s.AreaId}, {s.Row}, {s.Number})")));

        builder.Append("SET IDENTITY_INSERT dbo.Seat OFF;");

        builder.Append("SET IDENTITY_INSERT dbo.Event ON; ");
        builder.Append("INSERT INTO dbo.Event (Id, LayoutId, Name, Description, StartDate, EndDate, ImageUrl) ");
        builder.Append("VALUES ");

        builder.Append(string.Join(", ",
            Events.Select(e =>
                $"({e.Id}, {e.LayoutId}, '{e.Name}', '{e.Description}', '{e.StartDate:s}', '{e.EndDate:s}', '{e.ImageUrl}')")));

        builder.Append("SET IDENTITY_INSERT dbo.Event OFF;");

        builder.Append("SET IDENTITY_INSERT dbo.EventArea ON; ");
        builder.Append("INSERT INTO dbo.EventArea (Id, EventId, Description, CoordX, CoordY, Price) ");
        builder.Append("VALUES ");

        builder.Append(string.Join(", ",
            EventAreas.Select(ea => $"({ea.Id}, {ea.EventId}, '{ea.Description}', {ea.CoordX}, {ea.CoordY}, {ea.Price})")));

        builder.Append("SET IDENTITY_INSERT dbo.EventArea OFF;");

        builder.Append("SET IDENTITY_INSERT dbo.EventSeat ON; ");
        builder.Append("INSERT INTO dbo.EventSeat (Id, EventAreaId, Row, Number, State) ");
        builder.Append("VALUES ");

        builder.Append(string.Join(", ",
            EventSeats.Select(es => $"({es.Id}, {es.EventAreaId}, {es.Row}, {es.Number}, {es.State})")));

        builder.Append("SET IDENTITY_INSERT dbo.EventSeat OFF;");

        return builder.ToString();
    }
}