using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TicketManagement.DataAccess.EF.Implementation.DI
{
    /// <summary>
    /// Manage dependencies for DAL.
    /// </summary>
    public interface IServiceCollectionForDal
    {
        /// <summary>
        /// Register dependencies for DAL.
        /// </summary>
        void RegisterDependencies(IConfiguration configuration, IServiceCollection services);
    }
}