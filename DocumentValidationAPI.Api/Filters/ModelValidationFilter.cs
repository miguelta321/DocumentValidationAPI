using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DocumentValidationAPI.Api.Filters
{
    public class ModelValidationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .Where(x => x.Value?.Errors.Any() == true)
                    .Select(x => new
                    {
                        field = x.Key,
                        errors = x.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                    })
                    .ToArray();

                var problem = new
                {
                    error = "ValidationError",
                    message = "The request contains invalid data",
                    errors
                };

                context.Result = new BadRequestObjectResult(problem);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
