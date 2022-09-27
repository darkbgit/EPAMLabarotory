using Microsoft.Extensions.DependencyInjection;

namespace TicketManagement.Core.EventManagement.Services.DI
{
    /// <summary>
    /// Manage dependencies for business logic.
    /// </summary>
    public class ServiceCollectionForServices : IServiceCollectionForServices
    {
        /// <summary>
        /// Register dependencies for business logic.
        /// </summary>
        public void RegisterDependencies(IServiceCollection services)
        {
            RegisterServices.Register(services);

            RegisterValidators.Register(services);
        }
    }
}