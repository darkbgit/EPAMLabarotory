using Microsoft.Extensions.DependencyInjection;
using TicketManagement.Core.EventManagement.Services.Interfaces;
using TicketManagement.Core.EventManagement.Services.Mapping;

namespace TicketManagement.Core.EventManagement.Services.DI
{
    internal static class RegisterServices
    {
        public static void Register(IServiceCollection services)
        {
            services.AddScoped<IEventAreaService, EventAreaService>();
            services.AddScoped<IEventSeatService, EventSeatService>();
            services.AddScoped<IVenueService, VenueService>();
            services.AddScoped<IEventService, EventService>();

            services.AddScoped<ILayoutService, LayoutService>();
            services.AddScoped<IAreaService, AreaService>();
            services.AddSingleton<IImageBase64Service, ImageBase64Service>();

            services.AddAutoMapper(typeof(MappingProfile));

            services.AddLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });
        }
    }
}