using AutoMapper;
using TicketManagement.Core.Public.DTOs.AreaDTOs;
using TicketManagement.Core.Public.DTOs.EventAreaDTOs;
using TicketManagement.Core.Public.DTOs.EventDTOs;
using TicketManagement.Core.Public.DTOs.OrderDTOs;
using TicketManagement.MVC.Models.ViewModels.Event;
using TicketManagement.MVC.Models.ViewModels.EventArea;
using TicketManagement.MVC.Models.ViewModels.Order;

namespace TicketManagement.MVC.Mapping;

public class MappingProfileMvc : Profile
{
    public MappingProfileMvc()
    {
        CreateMap<OrderTicketDto, TicketsBougthViewModel>();

        CreateMap<AreaWithSeatsNumberDto, CreateEventAreaViewModel>()
            .ForMember("AreaId", opt => opt.MapFrom(r => r.Id));

        CreateMap<CreateEventViewModel, EventForCreateDto>();

        CreateMap<CreateEventAreaViewModel, EventAreaForCreateDto>();

        CreateMap<EventDto, EditEventViewModel>();
        CreateMap<EditEventViewModel, EventDto>();

        CreateMap<EventWithVenueIdDto, EditEventViewModel>();
        CreateMap<EditEventViewModel, EventWithVenueIdDto>();

        CreateMap<EventWithLayoutsDto, EditEventViewModel>()
            .ForMember(x => x.Layouts, opt => opt.Ignore());
    }
}