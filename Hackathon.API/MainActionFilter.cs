using Hackathon.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ValidationException = FluentValidation.ValidationException;

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
                case ValidationException or Common.Exceptions.ValidationException:

                    problemDetails = new ProblemDetails
                    {
                        Type = "https://example.com/validation",
                        Detail = context.Exception.Message,
                        Title = "Ошибка валидации",
                        Status = 400
                    };

                    if (context.Exception is ValidationException fluentValidationException)
                        problemDetails.Detail = string.Join('\n', fluentValidationException.Errors);

                    break;

                case EntityNotFoundException:
                    problemDetails = new ProblemDetails
                    {
                        Type = "https://example.com/entity-not-found",
                        Detail = context.Exception.Message,
                        Title = "Объект не найден",
                        Status = 400
                    };
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