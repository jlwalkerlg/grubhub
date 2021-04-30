using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Web
{
    [ApiController]
    public abstract class Action : ControllerBase
    {
        private static readonly Dictionary<ErrorType, int> Statuses = new()
        {
            { ErrorType.BadRequest, 400 },
            { ErrorType.Unauthenticated, 401 },
            { ErrorType.Unauthorised, 403 },
            { ErrorType.NotFound, 404 },
            { ErrorType.ValidationError, 422 },
            { ErrorType.Internal, 500 },
        };

        protected IActionResult NotFound(string message)
        {
            return Problem(Error.NotFound(message));
        }

        protected IActionResult Unauthorised(string message = null)
        {
            return Problem(Error.Unauthorised(message));
        }

        protected IActionResult Problem(Error error)
        {
            var problem = GetProblemDetails(error);

            return problem is ValidationProblemDetails validationProblemDetails
                ? ValidationProblem(validationProblemDetails)
                : Problem(problem.Detail, problem.Instance, problem.Status, problem.Title, problem.Type);
        }

        private static ProblemDetails GetProblemDetails(Error error)
        {
            if (error.Type == ErrorType.ValidationError)
            {
                return new ValidationProblemDetails(
                    error.Errors.ToDictionary(
                        x => ToCamelCase(x.Key),
                        x => new[] {x.Value}));
            }

            if (!Statuses.TryGetValue(error.Type, out var status))
            {
                status = 500;
            }

            return new ProblemDetails()
            {
                Status = status,
                Detail = error.Message,
            };
        }

        private static string ToCamelCase(string err)
        {
            return err.Substring(0, 1).ToLower() + err.Substring(1);
        }
    }
}
