using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Banking.Customers.Attributes
{
    //public class ModelValidationAttribute : ActionFilterAttribute
    //{
    //    public override void OnActionExecuting(ActionExecutingContext context)
    //    {
    //        if (!context.ModelState.IsValid)
    //        {
    //            context.Result = new StatusResult(context.ModelState);
    //        }
    //    }
    //}
    //public class StatusResult : ObjectResult
    //{
    //    public StatusResult(ModelStateDictionary modelState)
    //        : base(new Status(modelState, StatusCodes.Status422UnprocessableEntity))
    //    {
    //        StatusCode = StatusCodes.Status422UnprocessableEntity;
    //    }
    //}
    //public class Status
    //{
    //    public int Code { get; }
    //    public string Message { get; }
    //    public string Description { get; }

    //    public Status(ModelStateDictionary modelState, int errorStatusCode)
    //    {
    //        Code = errorStatusCode;
    //        Message = "Validation Failed";
    //        List<string> errors = modelState.Keys
    //            .SelectMany(key => modelState[key].Errors.Select(x => $"{key}: {(!string.IsNullOrWhiteSpace(x.ErrorMessage) ? x.ErrorMessage : "")}, "))
    //            .ToList();
    //        foreach (var error in errors) Description += error;
    //        Description = Description.TrimEnd().TrimEnd(',');
    //    }
    //}
}
