using Microsoft.Extensions.DependencyInjection;

namespace TicketManagement.DataAccess.Public.DI
{
    /// <summary>
    /// Manage dependencies for public models.
    /// </summary>
    public class ServiceCollectionForPublic : IServiceCollectionForPublic
    {
        /// <summary>
        /// Register dependencies for public models.
        /// </summary>
        public void RegisterDependencies(IServiceCollection services)
        {
            services.AddLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });
        }
    }
}