using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace TicketManagement.EventManagement.API.Helpers
{
    public class CustomTokenValidator : ISecurityTokenValidator
    {
        public CustomTokenValidator(string authenticationScheme)
        {
            AuthenticationScheme = authenticationScheme;
        }

        public string AuthenticationScheme { get; }

        public bool CanValidateToken => true;

        public int MaximumTokenSizeInBytes { get; set; }

        public bool CanReadToken(string securityToken) => true;

        public ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters, out SecurityToken? validatedToken)
        {
            validatedToken = null;

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "response.UserId"),
                new Claim(ClaimTypes.Email, "response.Email"),
                new Claim(ClaimsIdentity.DefaultNameClaimType, "response.UserName"),
            };

            return new ClaimsPrincipal(new ClaimsIdentity(claims, AuthenticationScheme));
        }
    }
}