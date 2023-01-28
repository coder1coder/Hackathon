using System.Linq;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Hackathon.API
{
    public class MainActionFilter: IActionFilter
    {

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is null) return;

            var problemDetails = new ProblemDetails
            {
                Type = "https://example.com/unhandled",
                Detail = context.Exception.Message,
                Title = "Неизвестная ошибка",
                Status = 500
            };

            switch (context.Exception)
            {
                case ValidationException:

                    problemDetails = new ProblemDetails
                    {
                        Type = "https://example.com/validation",
                        Detail = context.Exception.Message,
                        Title = "Ошибка валидации",
                        Status = 400
                    };

                    if (context.Exception is ValidationException fluentValidationException
                        && fluentValidationException.Errors.Any())
                        problemDetails.Detail = string.Join('\n', fluentValidationException.Errors);

                    break;
            }

            context.Result = new ObjectResult(problemDetails)
            {
                ContentTypes = {"application/problem+json"},
                StatusCode = problemDetails.Status
            };

            context.ExceptionHandled = true;
        }
    }
}
