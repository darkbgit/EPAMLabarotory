using Microsoft.Extensions.DependencyInjection;

namespace TicketManagement.Core.OrderManagement.Services.DI
{
    public interface IServiceCollectionForOrders
    {
        void RegisterDependencies(IServiceCollection services);
    }
}