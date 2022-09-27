using System.Data.Common;
using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using TicketManagement.Core.OrderManagement.Services.Interfaces;
using TicketManagement.Core.Public.DTOs.OrderDTOs;
using TicketManagement.Core.Public.Enums;
using TicketManagement.Core.Public.Exceptions;
using TicketManagement.Core.Public.Localization;
using TicketManagement.DataAccess.EF.Core.Entities;
using TicketManagement.DataAccess.Public;

namespace TicketManagement.Core.OrderManagement.Services
{
    internal sealed class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderService> _logger;
        private readonly IValidator<Order> _validator;
        private readonly IStringLocalizer<SharedResource> _serviceResourcesStringLocalizer;
        private readonly IStringLocalizer<OrderService> _stringLocalizer;

        public OrderService(IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<OrderService> logger,
            IValidator<Order> validator,
            IStringLocalizer<SharedResource> serviceResourcesStringLocalizer,
            IStringLocalizer<OrderService> stringLocalizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _serviceResourcesStringLocalizer = serviceResourcesStringLocalizer;
            _stringLocalizer = stringLocalizer;
            _validator = validator;
        }

        public async Task<OrderTicketDto> CreateAsync(OrderForCreateDto orderForCreateDto, CancellationToken cancellationToken = default)
        {
            var eventSeatIsExist = await _unitOfWork.EventSeat.IsExistWithStateAsync(orderForCreateDto.EventSeatId,
                (int)SeatState.Free, cancellationToken);

            if (!eventSeatIsExist)
            {
                throw new ServiceException(_stringLocalizer["error.EventSeatExists"]);
            }

            var order = _mapper.Map<Order>(orderForCreateDto);

            await _validator.ValidateAndThrowAsync(order, cancellationToken);

            _unitOfWork.Order.CreateOrder(order);
            await _unitOfWork.EventSeat.UpdateStateAsync(orderForCreateDto.EventSeatId, (int)SeatState.Occupied, cancellationToken);

            try
            {
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                var result = await _unitOfWork.Order.GetTicketInfoByIdAsync(order.Id, cancellationToken);
                return result;
            }
            catch (DbException e)
            {
                _logger.LogError(e.Message);
                throw new ServiceException(_serviceResourcesStringLocalizer["error.Add"], e);
            }
        }

        public async Task<IEnumerable<OrderTicketDto>> GetOrdersByUserId(Guid userId, CancellationToken cancellationToken = default)
        {
            var result = await _unitOfWork.Order
                .GetOrdersByUserId(userId)
                .ToListAsync(cancellationToken);

            return result;
        }
    }
}