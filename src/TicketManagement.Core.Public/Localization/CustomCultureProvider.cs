using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using TicketManagement.Core.Public.Extensions;

namespace TicketManagement.Core.Public.Localization
{
    public class CustomCultureProvider : RequestCultureProvider
    {
        public override Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            if (!httpContext.User.Identity!.IsAuthenticated)
            {
                return NullProviderCultureResult;
            }

            var culture = httpContext.User.GetCulture();

            if (string.IsNullOrWhiteSpace(culture))
            {
                return NullProviderCultureResult;
            }

            return Task.FromResult(new ProviderCultureResult(culture));
        }
    }
}