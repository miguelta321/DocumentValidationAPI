using DocumentValidationAPI.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class ValidationExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is ValidationException ex)
        {
            context.Result = new BadRequestObjectResult(new
            {
                error = "ValidationError",
                message = ex.Message
            });

            context.ExceptionHandled = true;
        }
    }
}
