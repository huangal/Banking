using Banking.Customers.Middleware;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Banking.Customers.Filters
{
    public class TrackPerformanceFilter : IActionFilter
    {
        private PerformanceTracker _tracker;
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!(context.ActionDescriptor is ControllerActionDescriptor actionDescriptor)) return;

            //_tracker = new PerformanceTracker($"{actionDescriptor.ControllerName}/{actionDescriptor.ActionName}");
            _tracker = new PerformanceTracker($"{actionDescriptor.ActionName}:{context.HttpContext.Request.Path}");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _tracker?.Stop();
        }
    }
}
