using Microsoft.Extensions.DependencyInjection;

namespace TicketManagement.DataAccess.Public.DI
{
    /// <summary>
    /// Manage dependencies for public models.
    /// </summary>
    public interface IServiceCollectionForPublic
    {
        /// <summary>
        /// Register dependencies for public models.
        /// </summary>
        void RegisterDependencies(IServiceCollection services);
    }
}