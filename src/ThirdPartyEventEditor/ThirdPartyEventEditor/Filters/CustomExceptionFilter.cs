using Serilog;
using System;
using System.Web.Mvc;

namespace ThirdPartyEventEditor.Filters
{
    public class CustomExceptionFilter : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled)
            {
                return;
            }

            var controllerName = filterContext.RouteData.Values["controller"].ToString();
            var actionName = filterContext.RouteData.Values["action"].ToString();

            Log.Error(filterContext.Exception, $"Exception while executing action {actionName} in controller {controllerName}.");

            var exceptionDetail = new ExceptionDetail
            {
                ExceptionMessage = filterContext.Exception.Message,
                StackTrace = filterContext.Exception.StackTrace,
                ControllerName = controllerName,
                ActionName = actionName,
                Date = DateTime.UtcNow,
            };

            var result = new ViewResult
            {
                ViewName = "Error",
                ViewData = new ViewDataDictionary
                {
                    Model = exceptionDetail,
                }
            };

            filterContext.Result = result;

            filterContext.ExceptionHandled = true;
        }
    }
}