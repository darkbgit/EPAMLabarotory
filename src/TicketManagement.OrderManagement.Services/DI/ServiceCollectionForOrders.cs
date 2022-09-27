using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TicketManagement.Core.OrderManagement.Services.Interfaces;
using TicketManagement.Core.OrderManagement.Services.Mapping;
using TicketManagement.Core.OrderManagement.Services.Validation;
using TicketManagement.DataAccess.EF.Core.Entities;

namespace TicketManagement.Core.OrderManagement.Services.DI
{
    public class ServiceCollectionForOrders : IServiceCollectionForOrders
    {
        public void RegisterDependencies(IServiceCollection services)
        {
            services.AddScoped<IOrderService, OrderService>();

            services.AddAutoMapper(typeof(OrdersMappingProfile));

            services.AddLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });

            services.AddScoped<IValidator<Order>, OrderValidator>();
        }
    }
}