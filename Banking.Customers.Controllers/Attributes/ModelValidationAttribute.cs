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

             var logs = modelState.Keys
                .SelectMany(key => modelState[key].Errors
                .Select(x => new { Name = key, ErrorMessage = !string.IsNullOrWhiteSpace(x.ErrorMessage) ? x.ErrorMessage : ""}))
                .ToList();

            foreach (var log in logs)
            {
                if(log.ErrorMessage.Contains("Error converting value", System.StringComparison.OrdinalIgnoreCase)
                    && log.ErrorMessage.Contains("System.Nullable", System.StringComparison.OrdinalIgnoreCase))
                    Description += $"{log.Name}: Invalid Data Type, ";
                else
                    Description += $"{log.Name}: {log.ErrorMessage}, ";
            }

            //List<string> errors = modelState.Keys
            //    .SelectMany(key => modelState[key].Errors.Select(x => $"{key}: {(!string.IsNullOrWhiteSpace(x.ErrorMessage) ? x.ErrorMessage : "")}, "))
            //    .ToList();
            //foreach (var error in errors) Description += error;
            Description = Description.TrimEnd().TrimEnd(',');
        }
    }
}
