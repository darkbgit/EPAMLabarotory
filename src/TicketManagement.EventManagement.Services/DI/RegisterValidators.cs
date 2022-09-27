using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TicketManagement.Core.EventManagement.Services.Validation;
using TicketManagement.DataAccess.EF.Core.Entities;

namespace TicketManagement.Core.EventManagement.Services.DI
{
    internal static class RegisterValidators
    {
        public static void Register(IServiceCollection services)
        {
            services.AddScoped<IValidator<Area>, AreaValidator>();
            services.AddScoped<IValidator<EventArea>, EventAreaValidator>();
            services.AddScoped<IValidator<EventSeat>, EventSeatValidator>();
            services.AddScoped<IValidator<IEnumerable<EventSeat>>, EventSeatListValidator>();
            services.AddScoped<IValidator<Layout>, LayoutValidator>();
            services.AddScoped<IValidator<Seat>, SeatValidator>();
            services.AddScoped<IValidator<IEnumerable<Seat>>, SeatListValidator>();
            services.AddScoped<IValidator<Venue>, VenueValidator>();
            services.AddScoped<IValidator<Event>, EventValidator>();

            services.AddScoped<EventServiceValidator>();
        }
    }
}