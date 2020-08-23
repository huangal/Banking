using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json;
using Banking.Customers.Domain.Interfaces;
using Banking.Customers.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Banking.Customers.Controllers.Attributes
{
    public class RequestValidationAttribute : ValidationAttribute
    {



        protected override ValidationResult IsValid( object value, ValidationContext validationContext)
        {
            if (value == null)
                return new ValidationResult("Invalid Data");
            return ValidationResult.Success; 

        //    string email = value as string;
        //    if (email == null)
        //        return new ValidationResult(Errors.General.ValueIsInvalid().Serialize());

        //    Result<Email> emailResult = Email.Create(email); 

        //if (emailResult.IsFailure)
        //        return new ValidationResult(emailResult.Error.Serialize()); 

        //return ValidationResult.Success;
        }


    }


    public class ModelValidationAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
              
                context.Result = new StatusResult(context);
            }
        }
    }

    public class StatusResult : ObjectResult
    {
        public StatusResult(ActionExecutingContext context)
            : base(new StatusResponse(context, StatusCodes.Status400BadRequest))
        {
            StatusCode = StatusCodes.Status400BadRequest;
        }
    }
          
    public class StatusResponse: ResponseStatus
    {
        public StatusResponse(ActionExecutingContext context, int errorStatusCode)
        {
            var param = context.ActionArguments.SingleOrDefault();
             var transaction = TransactionRequest.GetTransaction(context);

            ModelStateDictionary modelState = context.ModelState;
            TransactionId = (Guid)transaction.TransactionId;
            Status.Code = errorStatusCode;
            Status.Message = "Validation Failed";

            var errors = modelState.Keys
                .SelectMany(key => modelState[key].Errors.Select(x => GetErrorMessage(key, x.ErrorMessage)));

            foreach (string error in errors) Status.Description += error;

            Status.Description = Status.Description.TrimEnd().TrimEnd(',');
        }

        private string GetErrorMessage(string key, string message)
        {
            if (string.IsNullOrWhiteSpace(message)) return string.Empty;
            return message.Contains("Error converting value", System.StringComparison.OrdinalIgnoreCase)
                ? $"{key}: Invalid Data Type, "
                : $"{key}: {message}, ";
        }
    }

    public class TransactionRequest
    {
        public class Transaction
        {
            public Guid? TransactionId { get; set; } = Guid.NewGuid();
            
            public bool IsValidGuid()
            {
                return (Guid.TryParse(TransactionId.ToString(), out var guid) && guid != Guid.Empty);
            }
        }

        public static Transaction GetTransaction(ActionExecutingContext context)
        {
            Transaction transaction = new Transaction();

            if (context.ActionArguments.Keys.Any())
            {
                var value = context.ActionArguments.Values.FirstOrDefault();
                if (value != null)
                {
                    var requestBody = JsonSerializer.Serialize(value);
                    if (!string.IsNullOrWhiteSpace(requestBody))
                    {
                        transaction = JsonSerializer.Deserialize<Transaction>(requestBody);
                        if (!transaction.IsValidGuid()) transaction.TransactionId = Guid.NewGuid();
                    }
                }
            }
            return transaction;
        }
    }
}
