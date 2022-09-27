using Microsoft.Data.SqlClient;
using TicketManagement.DataAccess.EF.Core.Entities;

namespace TicketManagement.IntegrationTests.Helpers;

internal static class TestMapper
{
    public static Venue MapToVenue(SqlDataReader reader)
    {
        return new Venue
        {
            Id = reader.GetInt32(reader.GetOrdinal("Id")),
            Description = reader.GetString(reader.GetOrdinal("Description")),
            Address = reader.GetString(reader.GetOrdinal("Address")),
            Phone = !reader.IsDBNull(reader.GetOrdinal("Phone"))
                ? reader.GetString(reader.GetOrdinal("Phone"))
                : string.Empty,
        };
    }

    public static Layout MapToLayout(SqlDataReader reader)
    {
        return new Layout
        {
            Id = reader.GetInt32(reader.GetOrdinal("Id")),
            Description = reader.GetString(reader.GetOrdinal("Description")),
            VenueId = reader.GetInt32(reader.GetOrdinal("VenueId")),
        };
    }

    public static Area MapToArea(SqlDataReader reader)
    {
        return new Area
        {
            Id = reader.GetInt32(reader.GetOrdinal("Id")),
            Description = reader.GetString(reader.GetOrdinal("Description")),
            LayoutId = reader.GetInt32(reader.GetOrdinal("LayoutId")),
            CoordX = reader.GetInt32(reader.GetOrdinal("CoordX")),
            CoordY = reader.GetInt32(reader.GetOrdinal("CoordY")),
        };
    }

    public static Seat MapToSeat(SqlDataReader reader)
    {
        return new Seat
        {
            Id = reader.GetInt32(reader.GetOrdinal("Id")),
            Row = reader.GetInt32(reader.GetOrdinal("Row")),
            Number = reader.GetInt32(reader.GetOrdinal("Number")),
            AreaId = reader.GetInt32(reader.GetOrdinal("AreaId")),
        };
    }

    public static Event MapToEvent(SqlDataReader reader)
    {
        return new Event
        {
            Id = reader.GetInt32(reader.GetOrdinal("Id")),
            Name = reader.GetString(reader.GetOrdinal("Name")),
            Description = reader.GetString(reader.GetOrdinal("Description")),
            LayoutId = reader.GetInt32(reader.GetOrdinal("LayoutId")),
            StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
            EndDate = reader.GetDateTime(reader.GetOrdinal("EndDate")),
        };
    }

    public static EventArea MapToEventArea(SqlDataReader reader)
    {
        return new EventArea
        {
            Id = reader.GetInt32(reader.GetOrdinal("Id")),
            Description = reader.GetString(reader.GetOrdinal("Description")),
            EventId = reader.GetInt32(reader.GetOrdinal("EventId")),
            CoordX = reader.GetInt32(reader.GetOrdinal("CoordX")),
            CoordY = reader.GetInt32(reader.GetOrdinal("CoordY")),
            Price = reader.GetDecimal(reader.GetOrdinal("Price")),
        };
    }

    public static EventSeat MapToEventSeat(SqlDataReader reader)
    {
        return new EventSeat
        {
            Id = reader.GetInt32(reader.GetOrdinal("Id")),
            Row = reader.GetInt32(reader.GetOrdinal("Row")),
            Number = reader.GetInt32(reader.GetOrdinal("Number")),
            State = reader.GetInt32(reader.GetOrdinal("State")),
            EventAreaId = reader.GetInt32(reader.GetOrdinal("EventAreaId")),
        };
    }
}