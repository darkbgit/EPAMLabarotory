using Microsoft.AspNetCore.Http;
using TicketManagement.Core.Public.Constants;

namespace TicketManagement.Core.Public.Middlewares
{
    internal class RequestCultureFromHeaderMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestCultureFromHeaderMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Request.Headers.TryGetValue(CustomHeaders.LocaleHeader, out var locale);

            var culture = new System.Globalization.CultureInfo(locale.FirstOrDefault() ?? Cultures.English);

            Thread.CurrentThread.CurrentCulture = culture;

            await _next(context);
        }
    }
}
