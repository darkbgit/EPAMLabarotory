using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TicketManagement.Core.UserManagement.Services.DI
{
    /// <summary>
    /// Manage dependencies for user management.
    /// </summary>
    public interface IServiceCollectionForUserManagement
    {
        /// <summary>
        /// Register dependencies for user management.
        /// </summary>
        void RegisterDependencies(IConfiguration configuration, IServiceCollection services);
    }
}