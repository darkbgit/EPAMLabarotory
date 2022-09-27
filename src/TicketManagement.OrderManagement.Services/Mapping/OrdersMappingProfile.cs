using AutoMapper;
using TicketManagement.Core.Public.DTOs.OrderDTOs;
using TicketManagement.DataAccess.EF.Core.Entities;

namespace TicketManagement.Core.OrderManagement.Services.Mapping;

public class OrdersMappingProfile : Profile
{
    public OrdersMappingProfile()
    {
        CreateMap<OrderForCreateDto, Order>();
    }
}