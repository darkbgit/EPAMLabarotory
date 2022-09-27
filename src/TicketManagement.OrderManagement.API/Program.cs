using System.Reflection;
using Microsoft.OpenApi.Models;
using Refit;
using TicketManagement.Core.OrderManagement.Services.DI;
using TicketManagement.Core.Public.Clients;
using TicketManagement.Core.Public.Extensions;
using TicketManagement.Core.Public.Handlers;
using TicketManagement.DataAccess.EF.Implementation.DI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyMethod().AllowAnyHeader().AllowCredentials().WithOrigins(builder.Configuration["ReactApp"]);
    });
});

// Add services to the container.
builder.Services.AddControllers();

IServiceCollectionForDal serviceCollectionForDal = new ServiceCollectionForDal();
serviceCollectionForDal.RegisterDependencies(builder.Configuration, builder.Services);

IServiceCollectionForOrders serviceCollectionForOrders = new ServiceCollectionForOrders();
serviceCollectionForOrders.RegisterDependencies(builder.Services);

builder.Services.AddScoped<CustomHttpMessageHandler>();

builder.Services.AddRefitClient<IAuthClient>()
    .AddHttpMessageHandler<CustomHttpMessageHandler>()
    .ConfigureHttpClient(client => client.BaseAddress = new Uri(builder.Configuration["AuthClient"]));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config =>
{
    config.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API for Orders",
        Version = "v1",
        Description = "API for works with orders. Endpoints required Authorization in User role.",
    });

    config.AddAuthorizationButton();

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    config.IncludeXmlComments(xmlPath);
});

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCustomExceptionMiddleware();

app.UseHttpsRedirection();

app.UseAuthFromRequestHeaderMiddleware();

app.UseRequestCultureFromHeader();

app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.UseCustomRequestLocalization();

app.MapControllers();

app.Run();
