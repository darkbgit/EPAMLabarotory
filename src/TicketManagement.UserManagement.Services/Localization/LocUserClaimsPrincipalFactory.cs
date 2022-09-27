using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using TicketManagement.Core.Public.Constants;
using TicketManagement.DataAccess.EF.Core.Entities;

namespace TicketManagement.Core.UserManagement.Services.Localization
{
    internal class LocUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<User, Role>
    {
        public LocUserClaimsPrincipalFactory(
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IOptions<IdentityOptions> optionsAccessor)
            : base(userManager, roleManager, optionsAccessor)
        {
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
        {
            var identity = await base.GenerateClaimsAsync(user);

            identity.AddClaim(new Claim(CustomClaimTypes.Culture, user.Language ?? ""));
            identity.AddClaim(new Claim(CustomClaimTypes.TimeZoneId, user.TimeZoneId ?? ""));

            return identity;
        }
    }
}