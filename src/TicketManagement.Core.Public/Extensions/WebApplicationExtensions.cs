using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using TicketManagement.Core.Public.Constants;
using TicketManagement.Core.Public.Localization;
using TicketManagement.Core.Public.Middlewares;

namespace TicketManagement.Core.Public.Extensions;

public static class WebApplicationExtensions
{
    /// <summary>
    /// Add ExceptionMiddleware to pipeline.
    /// </summary>
    public static void UseCustomExceptionMiddleware(this WebApplication app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
    }

    public static void UseAuthFromRequestHeaderMiddleware(this WebApplication app)
    {
        app.UseMiddleware<AuthFromRequestHeaderMiddleware>();
    }

    public static void UseRequestCultureFromHeader(this WebApplication app)
    {
        app.UseMiddleware<RequestCultureFromHeaderMiddleware>();
    }

    public static void UseCustomRequestLocalization(this WebApplication app)
    {
        app.UseRequestLocalization(options =>
        {
            options.DefaultRequestCulture = new RequestCulture(Cultures.English);

            var cultures = new CultureInfo[]
            {
                new (Cultures.English),
                new (Cultures.Belarus),
                new (Cultures.Russian),
            };

            options.SupportedCultures = cultures;
            options.SupportedUICultures = cultures;

            options.AddInitialRequestCultureProvider(new CustomCultureProvider());
        });
    }
}