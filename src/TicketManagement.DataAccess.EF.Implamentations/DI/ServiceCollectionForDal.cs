using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TicketManagement.DataAccess.EF.Core;
using TicketManagement.DataAccess.Public;

namespace TicketManagement.DataAccess.EF.Implementation.DI
{
    /// <summary>
    /// Manage dependencies for DAL.
    /// </summary>
    public sealed class ServiceCollectionForDal : IServiceCollectionForDal
    {
        /// <summary>
        /// Register dependencies for DAL.
        /// </summary>
        public void RegisterDependencies(IConfiguration configuration, IServiceCollection services)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<TicketManagementContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}