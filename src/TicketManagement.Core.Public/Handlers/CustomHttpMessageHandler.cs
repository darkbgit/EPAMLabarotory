using System.Net.Http.Headers;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using TicketManagement.Core.Public.Constants;

namespace TicketManagement.Core.Public.Handlers;

public class CustomHttpMessageHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CustomHttpMessageHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var isAuthenticated = _httpContextAccessor.HttpContext?.User.Identity is { IsAuthenticated: true };

        if (!isAuthenticated)
        {
            return await base.SendAsync(request, cancellationToken);
        }

        string token = null;

        token = _httpContextAccessor.HttpContext?.User.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.Authentication)
            ?.Value;

        if (token != null)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, token);
        }

        var locale = _httpContextAccessor.HttpContext?.User.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.Locality)
            ?.Value;

        request.Headers.Add(CustomHeaders.LocaleHeader, locale);

        return await base.SendAsync(request, cancellationToken);
    }
}
