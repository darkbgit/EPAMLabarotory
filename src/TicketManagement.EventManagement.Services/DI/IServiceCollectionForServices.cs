using Microsoft.Extensions.DependencyInjection;

namespace TicketManagement.Core.EventManagement.Services.DI
{
    /// <summary>
    /// Manage dependencies for business logic.
    /// </summary>
    public interface IServiceCollectionForServices
    {
        /// <summary>
        /// Register dependencies for business logic.
        /// </summary>
        void RegisterDependencies(IServiceCollection services);
    }
}