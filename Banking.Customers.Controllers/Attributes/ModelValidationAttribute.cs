using System.Collections.Generic;
using System.Linq;
using Banking.Customers.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Banking.Customers.Controllers.Attributes
{

    public class ModelValidationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new StatusResult(context.ModelState);
            }
        }
    }
    public class StatusResult : ObjectResult
    {
        public StatusResult(ModelStateDictionary modelState)
            : base(new StatusResponse(modelState, StatusCodes.Status422UnprocessableEntity))
        {
            StatusCode = StatusCodes.Status422UnprocessableEntity;
        }
    }
    public class StatusResponse: Status
    {
        public StatusResponse(ModelStateDictionary modelState, int errorStatusCode)
        {
            Code = errorStatusCode;
            Message = "Validation Failed";

            var errors = modelState.Keys
                .SelectMany(key => modelState[key].Errors.Select(x => GetErrorMessage(key, x.ErrorMessage)));

            foreach (string error in errors) Description += error;

            Description = Description.TrimEnd().TrimEnd(',');
        }

        private string GetErrorMessage(string key, string message)
        {
            if (string.IsNullOrWhiteSpace(message)) return string.Empty;

            return message.Contains("Error converting value", System.StringComparison.OrdinalIgnoreCase)
                   && message.Contains("System.Nullable", System.StringComparison.OrdinalIgnoreCase)
                ? $"{key}: Invalid Data Type, "
                : $"{key}: {message}, ";
        }

    }
}
