using Autofac;
using Autofac.Integration.Mvc;
using Serilog;
using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ThirdPartyEventEditor.Repositories;

namespace ThirdPartyEventEditor
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var dataDirectoryPath = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
            var logFileName = ConfigurationManager.AppSettings["LogFile"];
            var logPath = Path.Combine(dataDirectoryPath, logFileName);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
                //.Enrich.WithExceptionDetails()
                .Enrich.FromLogContext()
                .WriteTo.File(logPath, Serilog.Events.LogEventLevel.Information)
                .CreateLogger();

            var builder = new ContainerBuilder();

            var dbJsonFileName = ConfigurationManager.AppSettings["DbJsonFile"];
            var dbJsonPath = Path.Combine(dataDirectoryPath, dbJsonFileName);

            builder.Register(t => new EventRepository(dbJsonPath))
                .As<IEventRepository>()
                .InstancePerRequest();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => t.Name.EndsWith("Controller"));

            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}