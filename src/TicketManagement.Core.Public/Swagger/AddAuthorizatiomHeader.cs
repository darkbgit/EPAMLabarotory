using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using TicketManagement.Core.Public.Filters;

namespace TicketManagement.Core.Public.Swagger;

public class AddAuthorizatiomHeader : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation == null)
        {
            return;
        }

        operation.Parameters ??= new List<OpenApiParameter>();

        var authorizeAttributes = context.ApiDescription
            .CustomAttributes()
            .OfType<AuthorizeAttribute>();

        var allowAnonymousAttributes = context.ApiDescription
            .CustomAttributes()
            .OfType<AllowAnonymousAttribute>();

        if (!authorizeAttributes.Any() && allowAnonymousAttributes.Any())
        {
            return;
        }

        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "Authorization",
            In = ParameterLocation.Header,
        });
    }
}
