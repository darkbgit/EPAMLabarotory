using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TicketManagement.Core.UserManagement.Services.Interfaces;
using TicketManagement.Core.UserManagement.Services.Localization;
using TicketManagement.Core.UserManagement.Services.Mapping;
using TicketManagement.DataAccess.EF.Core;
using TicketManagement.DataAccess.EF.Core.Entities;

namespace TicketManagement.Core.UserManagement.Services.DI
{
    /// <summary>
    /// Manage dependencies for user management.
    /// </summary>
    public class ServiceCollectionForUserManagement : IServiceCollectionForUserManagement
    {
        /// <summary>
        /// Register dependencies for user management.
        /// </summary>
        public void RegisterDependencies(IConfiguration configuration, IServiceCollection services)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<TicketManagementContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddIdentity<User, Role>(options =>
                    options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<TicketManagementContext>()
                .AddClaimsPrincipalFactory<LocUserClaimsPrincipalFactory>();

            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IJwtService, JwtService>();

            services.AddAutoMapper(typeof(UserManagementMappingProfile));

            services.AddLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });
        }
    }
}