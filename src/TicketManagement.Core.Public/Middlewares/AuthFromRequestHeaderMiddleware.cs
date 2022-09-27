using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Refit;
using TicketManagement.Core.Public.Clients;
using TicketManagement.Core.Public.Constants;
using TicketManagement.Core.Public.Requests.UserManagement;
using TicketManagement.Core.Public.Responses.UserManagement;

namespace TicketManagement.Core.Public.Middlewares
{
    internal class AuthFromRequestHeaderMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IAuthClient _authClient;

        public AuthFromRequestHeaderMiddleware(RequestDelegate next,
            IAuthClient authClient)
        {
            _next = next;
            _authClient = authClient;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var authHeader = context.Request.Headers[HeaderNames.Authorization].FirstOrDefault();

            if (string.IsNullOrEmpty(authHeader))
            {
                await _next(context);
                return;
            }

            var token = authHeader.Split(" ").LastOrDefault();

            if (string.IsNullOrEmpty(token))
            {
                await _next(context);
                return;
            }

            var request = new TokenRequest
            {
                Token = token,
            };

            var isValid = false;

            try
            {
                isValid = await _authClient.ValidateToken(request);
            }
            catch (ApiException)
            {
                await _next(context);
                return;
            }

            if (!isValid)
            {
                await _next(context);
                return;
            }

            UserInfoResponse userInfoResponse = null;

            try
            {
                userInfoResponse = await _authClient.GetUserInfo(request);
            }
            catch (ApiException)
            {
                await _next(context);
                return;
            }

            if (userInfoResponse == null)
            {
                await _next(context);
                return;
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Authentication, authHeader),
                new Claim(ClaimTypes.NameIdentifier, userInfoResponse.Id.ToString()),
                new Claim(ClaimTypes.Locality, userInfoResponse.Locale ?? Cultures.English),
            };

            var identity = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);

            foreach (var userInfoRole in userInfoResponse.Roles)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, userInfoRole));
            }

            context.User = new ClaimsPrincipal(identity);

            await _next(context);
        }
    }
}