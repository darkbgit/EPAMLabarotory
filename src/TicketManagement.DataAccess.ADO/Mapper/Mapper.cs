using System;
using Microsoft.Data.SqlClient;
using TicketManagement.DataAccess.EF.Core.Entities;

namespace TicketManagement.DataAccess.ADO.Mapper
{
    internal static class Mapper
    {
        public static T Map<T>(SqlDataReader reader)
            where T : class, IBaseEntity
        {
            var result = Activator.CreateInstance<T>();

            switch (result)
            {
                case Venue venue:
                    venue.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                    venue.Description = reader.GetString(reader.GetOrdinal("Description"));
                    venue.Address = reader.GetString(reader.GetOrdinal("Address"));
                    venue.Phone = !reader.IsDBNull(reader.GetOrdinal("Phone"))
                        ? reader.GetString(reader.GetOrdinal("Phone"))
                        : string.Empty;
                    break;
                case Layout layout:
                    layout.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                    layout.Description = reader.GetString(reader.GetOrdinal("Description"));
                    layout.VenueId = reader.GetInt32(reader.GetOrdinal("VenueId"));
                    break;
                case Area area:
                    area.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                    area.Description = reader.GetString(reader.GetOrdinal("Description"));
                    area.LayoutId = reader.GetInt32(reader.GetOrdinal("LayoutId"));
                    area.CoordX = reader.GetInt32(reader.GetOrdinal("CoordX"));
                    area.CoordY = reader.GetInt32(reader.GetOrdinal("CoordY"));
                    break;
                case Seat seat:
                    seat.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                    seat.Row = reader.GetInt32(reader.GetOrdinal("Row"));
                    seat.Number = reader.GetInt32(reader.GetOrdinal("Number"));
                    seat.AreaId = reader.GetInt32(reader.GetOrdinal("AreaId"));
                    break;
                case Event ev:
                    ev.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                    ev.Name = reader.GetString(reader.GetOrdinal("Name"));
                    ev.Description = reader.GetString(reader.GetOrdinal("Description"));
                    ev.LayoutId = reader.GetInt32(reader.GetOrdinal("LayoutId"));
                    ev.StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate"));
                    ev.EndDate = reader.GetDateTime(reader.GetOrdinal("EndDate"));
                    break;
                case EventArea eventArea:
                    eventArea.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                    eventArea.Description = reader.GetString(reader.GetOrdinal("Description"));
                    eventArea.EventId = reader.GetInt32(reader.GetOrdinal("EventId"));
                    eventArea.CoordX = reader.GetInt32(reader.GetOrdinal("CoordX"));
                    eventArea.CoordY = reader.GetInt32(reader.GetOrdinal("CoordY"));
                    eventArea.Price = reader.GetDecimal(reader.GetOrdinal("Price"));
                    break;
                case EventSeat eventSeat:
                    eventSeat.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                    eventSeat.Row = reader.GetInt32(reader.GetOrdinal("Row"));
                    eventSeat.Number = reader.GetInt32(reader.GetOrdinal("Number"));
                    eventSeat.State = reader.GetInt32(reader.GetOrdinal("State"));
                    eventSeat.EventAreaId = reader.GetInt32(reader.GetOrdinal("EventAreaId"));
                    break;
                default:
                    throw new ArgumentException("Invalid type in method Map.");
            }

            return result;
        }
    }
}