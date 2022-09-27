using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TicketManagement.Core.Public.DTOs.EventDTOs;
using TicketManagement.Core.Public.DTOs.LayoutDTOs;
using TicketManagement.Core.Public.Enums;
using TicketManagement.DataAccess.EF.Core;
using TicketManagement.DataAccess.EF.Core.Entities;
using TicketManagement.DataAccess.EF.Implementation.Repositories.Base;
using TicketManagement.DataAccess.Public.Repositories;

namespace TicketManagement.DataAccess.EF.Implementation.Repositories
{
    internal sealed class EventRepository : Repository<Event>, IEventRepository
    {
        public EventRepository(TicketManagementContext context)
            : base(context)
        {
        }

        public int CreateEvent(Event entity)
        {
            var param = new SqlParameter[]
            {
                new SqlParameter
                {
                    ParameterName = "@layoutId",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Input,
                    Value = entity.LayoutId,
                },
                new SqlParameter
                {
                    ParameterName = "@description",
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                    Direction = System.Data.ParameterDirection.Input,
                    Value = entity.Description,
                },
                new SqlParameter
                {
                    ParameterName = "@name",
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                    Size = 120,
                    Direction = System.Data.ParameterDirection.Input,
                    Value = entity.Name,
                },
                new SqlParameter
                {
                    ParameterName = "@startDate",
                    SqlDbType = System.Data.SqlDbType.DateTime2,
                    Precision = 7,
                    Direction = System.Data.ParameterDirection.Input,
                    Value = entity.StartDate,
                },
                new SqlParameter
                {
                    ParameterName = "@endDate",
                    SqlDbType = System.Data.SqlDbType.DateTime2,
                    Precision = 7,
                    Direction = System.Data.ParameterDirection.Input,
                    Value = entity.EndDate,
                },
                new SqlParameter
                {
                    ParameterName = "@imageUrl",
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                    Direction = System.Data.ParameterDirection.Input,
                    Value = entity.ImageUrl,
                },
                new SqlParameter
                {
                    ParameterName = "@id",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Output,
                },
            };

            Context.Database.ExecuteSqlRaw(
                "[dbo].[event_Add] @layoutId, @description, @name, @startDate, @endDate, @imageUrl, @id out", param);

            int id = Convert.ToInt32(param.First(p => p.ParameterName == "@id").Value);

            return id;
        }

        public int DeleteEvent(int eventId)
        {
            var param = new SqlParameter[]
            {
                new SqlParameter
                {
                    ParameterName = "@id",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Input,
                    Value = eventId,
                },
                new SqlParameter
                {
                    ParameterName = "@rowCount",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Output,
                },
            };

            Context.Database.ExecuteSqlRaw(
                "[dbo].[event_Remove] @id, @rowCount out", param);

            int rowCount = Convert.ToInt32(param.First(p => p.ParameterName == "@rowCount").Value);

            return rowCount;
        }

        public async Task<bool> IsExistAsync(Event entity, CancellationToken cancellationToken)
        {
            var result = await Table.Where(@event => @event.LayoutId == entity.LayoutId)
                .Where(@event => @event.Name == entity.Name || @event.Description == entity.Description ||
                        (@event.EndDate > entity.StartDate && @event.EndDate < entity.EndDate))
                .FirstOrDefaultAsync(cancellationToken);

            return result != null;
        }

        public async Task<bool> IsExistForUpdateAsync(Event entity, CancellationToken cancellationToken)
        {
            var result = await Table
                .Where(@event => @event.LayoutId == entity.LayoutId && @event.Id != entity.Id)
                .Where(@event => @event.Name == entity.Name || @event.Description == entity.Description ||
                        (@event.EndDate > entity.StartDate && @event.EndDate < entity.EndDate))
                .FirstOrDefaultAsync(cancellationToken);

            return result != null;
        }

        public int UpdateEvent(Event entity)
        {
            var param = new SqlParameter[]
            {
                new SqlParameter
                {
                    ParameterName = "@id",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Input,
                    Value = entity.Id,
                },
                new SqlParameter
                {
                    ParameterName = "@layoutId",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Input,
                    Value = entity.LayoutId,
                },
                new SqlParameter
                {
                    ParameterName = "@description",
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                    Direction = System.Data.ParameterDirection.Input,
                    Value = entity.Description,
                },
                new SqlParameter
                {
                    ParameterName = "@name",
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                    Size = 120,
                    Direction = System.Data.ParameterDirection.Input,
                    Value = entity.Name,
                },
                new SqlParameter
                {
                    ParameterName = "@startDate",
                    SqlDbType = System.Data.SqlDbType.DateTime2,
                    Precision = 7,
                    Direction = System.Data.ParameterDirection.Input,
                    Value = entity.StartDate,
                },
                new SqlParameter
                {
                    ParameterName = "@endDate",
                    SqlDbType = System.Data.SqlDbType.DateTime2,
                    Precision = 7,
                    Direction = System.Data.ParameterDirection.Input,
                    Value = entity.EndDate,
                },
                new SqlParameter
                {
                    ParameterName = "@imageUrl",
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                    Direction = System.Data.ParameterDirection.Input,
                    Value = entity.ImageUrl,
                },
                new SqlParameter
                {
                    ParameterName = "@rowCount",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Output,
                },
            };

            Context.Database.ExecuteSqlRaw(
                "[dbo].[event_Update] @id, @layoutId, @description, @name, @startDate, @endDate, @imageUrl, @rowCount out", param);

            int rowCount = Convert.ToInt32(param.First(i => i.ParameterName == "@rowCount").Value);

            return rowCount;
        }

        public IQueryable<EventWithVenueIdDto> GetEventsWithVenueIdById()
        {
            var query = Table
                .Join(Context.Layout, @event => @event.LayoutId, layout => layout.Id,
                (@event, layout) => new EventWithVenueIdDto
                {
                    Id = @event.Id,
                    Name = @event.Name,
                    Description = @event.Description,
                    StartDate = @event.StartDate,
                    EndDate = @event.EndDate,
                    ImageUrl = @event.ImageUrl,
                    LayoutId = @event.LayoutId,
                    VenueId = layout.VenueId,
                });

            return query;
        }

        public IQueryable<EventWithLayoutsDto> GetEventsWithLayoutsAndVenueTimeZoneById()
        {
            var query = from @event in Table
                        join layout in Context.Layout on @event.LayoutId equals layout.Id
                        join venue in Context.Venue on layout.VenueId equals venue.Id
                        select new EventWithLayoutsDto
                        {
                            Id = @event.Id,
                            Name = @event.Name,
                            Description = @event.Description,
                            StartDate = @event.StartDate,
                            EndDate = @event.EndDate,
                            ImageUrl = @event.ImageUrl,
                            LayoutId = @event.LayoutId,
                            VenueTimeZoneId = venue.TimeZoneId,
                            VenueId = layout.VenueId,
                            Layouts =
                                from layouts in Context.Layout
                                where layouts.VenueId == venue.Id
                                select new LayoutDto
                                {
                                    Id = layouts.Id,
                                    Description = layouts.Description,
                                },
                        };

            return query;
        }

        public IQueryable<EventInfoForEventAreaListDto> GetEventsForEventAreaList()
        {
            var query = from @event in Table
                        join layout in Context.Layout on @event.LayoutId equals layout.Id
                        join venue in Context.Venue on layout.VenueId equals venue.Id
                        select new EventInfoForEventAreaListDto
                        {
                            Id = @event.Id,
                            EventName = @event.Name,
                            LayoutName = layout.Description,
                            StartDate = @event.StartDate,
                        };

            return query;
        }

        public IQueryable<EventForDetailsDto> GetEventsWithDetailsById()
        {
            var query = from @event in Table
                        join layout in Context.Layout on @event.LayoutId equals layout.Id
                        join venue in Context.Venue on layout.VenueId equals venue.Id
                        join eventArea in Context.EventArea on @event.Id equals eventArea.EventId
                        join eventSeat in Context.EventSeat.Where(eventSeat => eventSeat.State == (int)SeatState.Free) on eventArea.Id equals eventSeat.EventAreaId into eventSeats
                        from eventSeat in eventSeats.DefaultIfEmpty()
                        group eventSeat by new
                        {
                            @event.Id,
                            @event.ImageUrl,
                            @event.Name,
                            @event.Description,
                            LayoutDescription = layout.Description,
                            VenueDescription = venue.Description,
                            VenueAddress = venue.Address,
                            VenuePhone = venue.Phone,
                            @event.StartDate,
                            @event.EndDate,
                        }
                into grouping
                        select new EventForDetailsDto
                        {
                            Id = grouping.Key.Id,
                            ImageUrl = grouping.Key.ImageUrl,
                            Name = grouping.Key.Name,
                            Description = grouping.Key.Description,
                            LayoutDescription = grouping.Key.LayoutDescription,
                            VenueDescription = grouping.Key.VenueDescription,
                            VenueAddress = grouping.Key.VenueAddress,
                            VenuePhone = grouping.Key.VenuePhone,
                            StartDate = grouping.Key.StartDate,
                            Duration = grouping.Key.EndDate - grouping.Key.StartDate,
                            FreeSeats = grouping.Count(es => es != null),
                        };

            return query;
        }

        public IQueryable<EventForEditListDto> GetEventsForEditList()
        {
            var query = from @event in Table
                        join layout in Context.Layout on @event.LayoutId equals layout.Id
                        join venue in Context.Venue on layout.VenueId equals venue.Id
                        join eventArea in Context.EventArea on @event.Id equals eventArea.EventId
                        group @event by new
                        {
                            @event.Id,
                            @event.Name,
                            LayoutDescription = layout.Description,
                            VenueDescription = venue.Description,
                            @event.StartDate,
                            @event.EndDate,
                        }
                        into grouping
                        select new EventForEditListDto
                        {
                            Id = grouping.Key.Id,
                            Name = grouping.Key.Name,
                            LayoutDescription = grouping.Key.LayoutDescription,
                            VenueDescription = grouping.Key.VenueDescription,
                            StartDate = grouping.Key.StartDate,
                            Duration = grouping.Key.EndDate - grouping.Key.StartDate,
                            EventAreasCount = grouping.Count(),
                        };

            return query;
        }

        public async Task<int> EventsForEditListCountAsync(string searchString, CancellationToken cancellationToken)
        {
            var isNeedSearch = !string.IsNullOrEmpty(searchString);

            var queryForCount = from @event in Table.Where(e => !isNeedSearch || e.Name.Contains(searchString))
                                join layout in Context.Layout.Where(l => !isNeedSearch || l.Description.Contains(searchString)) on @event.LayoutId equals layout.Id
                                join venue in Context.Venue.Where(v => !isNeedSearch || v.Description.Contains(searchString)) on layout.VenueId equals venue.Id
                                group @event by @event.Id;

            var count = await queryForCount.CountAsync(cancellationToken);

            return count;
        }

        public IQueryable<EventForListDto> GetEventsForMainPage()
        {
            var eventsIdWithNullPrice = Context.EventArea
                .Where(ea => ea.Price == null)
                .Select(ea => ea.EventId)
                .Distinct();

            var query = from @event in Table
                        join layout in Context.Layout on @event.LayoutId equals layout.Id
                        join venue in Context.Venue on layout.VenueId equals venue.Id
                        join eventArea in Context.EventArea
                            on @event.Id
                            equals eventArea.EventId
                            into eventAreas
                        from eventArea in eventAreas.DefaultIfEmpty()
                        join eventSeat in Context.EventSeat
                            on new { eventAreaId = eventArea.Id, eventSeatState = (int)SeatState.Free }
                            equals new { eventAreaId = eventSeat.EventAreaId, eventSeatState = eventSeat.State }
                            into eventSeats
                        from eventSeat in eventSeats.DefaultIfEmpty()
                        where !eventsIdWithNullPrice.Contains(@event.Id)
                        group eventSeat by new
                        {
                            @event.Id,
                            @event.ImageUrl,
                            @event.Name,
                            LayoutDescription = layout.Description,
                            VenueDescription = venue.Description,
                            @event.StartDate,
                            @event.EndDate,
                        }
                into grouping
                        select new EventForListDto
                        {
                            Id = grouping.Key.Id,
                            ImageUrl = grouping.Key.ImageUrl,
                            Name = grouping.Key.Name,
                            LayoutDescription = grouping.Key.LayoutDescription,
                            VenueDescription = grouping.Key.VenueDescription,
                            StartDate = grouping.Key.StartDate,
                            Duration = grouping.Key.EndDate - grouping.Key.StartDate,
                            FreeSeats = grouping.Count(es => es != null),
                        };

            return query;
        }

        public async Task<int> EventsForManePageCountAsync(CancellationToken cancellationToken)
        {
            var eventsIdWithNullPrice = Context.EventArea
                .Where(ea => ea.Price == null)
                .Select(ea => ea.EventId)
                .Distinct();

            var query = from @event in Table
                        join layout in Context.Layout on @event.LayoutId equals layout.Id
                        join venue in Context.Venue on layout.VenueId equals venue.Id
                        join eventArea in Context.EventArea
                            on @event.Id
                            equals eventArea.EventId
                            into eventAreas
                        from eventArea in eventAreas.DefaultIfEmpty()
                        where !eventsIdWithNullPrice.Contains(@event.Id)
                        group @event by @event.Id;

            var count = await query.CountAsync(cancellationToken);

            return count;
        }
    }
}