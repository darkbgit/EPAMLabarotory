using System.Globalization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.FileProviders;
using Microsoft.FeatureManagement;
using Refit;
using Serilog;
using TicketManagement.Core.Public.Constants;
using TicketManagement.Core.Public.Extensions;
using TicketManagement.Core.Public.Handlers;
using TicketManagement.MVC.Clients.EventManagement;
using TicketManagement.MVC.Clients.OrderManagement;
using TicketManagement.MVC.Clients.UserManagement;
using TicketManagement.MVC.Mapping;
using TicketManagement.MVC.Utilities.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFeatureManagement();

var logFilePath = builder.Configuration["LogFilePath"];

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console(Serilog.Events.LogEventLevel.Debug)
    .WriteTo.File(logFilePath, Serilog.Events.LogEventLevel.Information)
    .CreateLogger();

builder.Services.AddScoped<CustomHttpMessageHandler>();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddRefitClient<IEventsClient>()
    .AddHttpMessageHandler<CustomHttpMessageHandler>()
    .ConfigureHttpClient(c =>
        c.BaseAddress = new Uri(builder.Configuration["EventsClient"]));

builder.Services.AddRefitClient<IEventAreasClient>()
    .AddHttpMessageHandler<CustomHttpMessageHandler>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.Configuration["EventsClient"]));

builder.Services.AddRefitClient<IEventSeatsClient>()
    .AddHttpMessageHandler<CustomHttpMessageHandler>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.Configuration["EventsClient"]));

builder.Services.AddRefitClient<IVenuesClient>()
    .AddHttpMessageHandler<CustomHttpMessageHandler>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.Configuration["EventsClient"]));

builder.Services.AddRefitClient<ILayoutsClient>()
    .AddHttpMessageHandler<CustomHttpMessageHandler>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.Configuration["EventsClient"]));

builder.Services.AddRefitClient<IUsersClient>()
    .AddHttpMessageHandler<CustomHttpMessageHandler>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.Configuration["UsersClient"]));

builder.Services.AddRefitClient<IOrdersClient>()
    .AddHttpMessageHandler<CustomHttpMessageHandler>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.Configuration["OrdersClient"]));

builder.Services.AddAutoMapper(typeof(MappingProfileMvc));

builder.Services.AddControllersWithViews()
    .AddDataAnnotationsLocalization()
    .AddViewLocalization();

builder.Services.AddRazorPages()
    .AddDataAnnotationsLocalization();

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var cultures = new CultureInfo[]
    {
                    new (Cultures.English),
                    new (Cultures.Belarus),
                    new (Cultures.Russian),
    };

    options.SupportedCultures = cultures;
    options.SupportedUICultures = cultures;
});

builder.Services.AddLocalization(options =>
{
    options.ResourcesPath = "Resources";
});

//var physicalProvider = new PhysicalFileProvider(builder.Configuration.GetValue<string>("StoredFilesPath"));

//builder.Services.AddSingleton<IFileProvider>(physicalProvider);
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

builder.Services.AddHttpContextAccessor();

builder.Services.AddSpaStaticFiles(configuration =>
{
    configuration.RootPath = "ClientApp/build";
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

var featureManager = app.Services.GetService<IFeatureManager>();

if (await featureManager.IsEnabledAsync("React"))
{
    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseSpaStaticFiles();

    app.UseRouting();

    app.UseSpa(spa =>
    {
        spa.Options.SourcePath = "ClientApp";

        if (app.Environment.IsDevelopment())
        {
            spa.UseReactDevelopmentServer(npmScript: "start");
        }
    });
}
else
{
    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseMiddleware<LoggingMiddleware>();

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseCustomRequestLocalization();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllerRoute(
            name: "default",
            pattern: "{controller=Event}/{action=Index}/{id?}");
        endpoints.MapRazorPages();
    });
}

app.Run();

namespace TicketManagement.MVC
{
    public partial class Program
    {
        protected Program()
        {
        }
    }
}