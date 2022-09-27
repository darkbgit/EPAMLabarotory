using System.Security.Claims;
using TicketManagement.Core.Public.Constants;

namespace TicketManagement.MVC.Utilities.Extensions
{
    public static class ClaimsPrincipleExtension
    {
        public static string GetCulture(this ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal == null)
            {
                throw new ArgumentNullException(nameof(claimsPrincipal));
            }

            return claimsPrincipal.FindFirstValue(ClaimTypes.Locality);
        }

        public static string GetTimeZoneId(this ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal == null)
            {
                throw new ArgumentNullException(nameof(claimsPrincipal));
            }

            return claimsPrincipal.FindFirstValue(CustomClaimTypes.TimeZoneId);
        }
    }
}