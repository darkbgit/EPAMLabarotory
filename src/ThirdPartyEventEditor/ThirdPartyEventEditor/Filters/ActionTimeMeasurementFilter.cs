using Serilog;
using System.Diagnostics;
using System.Web.Mvc;

namespace ThirdPartyEventEditor.Filters
{
    public class ActionTimeMeasurementFilter : FilterAttribute, IActionFilter
    {
        private Stopwatch _stopwatch;

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            _stopwatch.Stop();

            var controllerName = filterContext.RouteData.Values["controller"].ToString();
            var actionName = filterContext.RouteData.Values["action"].ToString();

            var duration = _stopwatch.ElapsedMilliseconds;

            Log.Information($"Action {actionName} in controller {controllerName} completed in {duration} ms.");

            _stopwatch.Reset();
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _stopwatch = Stopwatch.StartNew();
        }
    }
}