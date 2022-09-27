using AutoMapper;
using TicketManagement.Core.Public.DTOs.AreaDTOs;
using TicketManagement.Core.Public.DTOs.EventAreaDTOs;
using TicketManagement.Core.Public.DTOs.EventDTOs;
using TicketManagement.Core.Public.DTOs.EventSeatDTOs;
using TicketManagement.Core.Public.DTOs.LayoutDTOs;
using TicketManagement.Core.Public.DTOs.VenueDTOs;
using TicketManagement.DataAccess.EF.Core.Entities;

namespace TicketManagement.Core.EventManagement.Services.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Venue, VenueDto>();
        CreateMap<VenueDto, Venue>();

        CreateMap<Event, EventForListDto>();

        CreateMap<EventSeat, EventSeatForListDto>();

        CreateMap<Layout, LayoutDto>();

        CreateMap<Area, AreaDto>();

        CreateMap<Event, EventDto>();
        CreateMap<EventDto, Event>();

        CreateMap<EventForCreateDto, Event>();

        CreateMap<EventForUpdateDto, Event>();

        CreateMap<EventAreaForCreateDto, EventArea>();

        CreateMap<EventAreaDto, EventArea>();
        CreateMap<EventArea, EventAreaDto>();

        CreateMap<EventSeatDto, EventSeat>();
        CreateMap<EventSeat, EventSeatDto>();

        CreateMap<Area, EventArea>()
            .ForMember(x => x.Id, opt => opt.Ignore());

        CreateMap<Seat, EventSeat>()
            .ForMember(x => x.Id, opt => opt.Ignore());

        CreateMap<ThirdPartyEventDto, Event>();
    }
}