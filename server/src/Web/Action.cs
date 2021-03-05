using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Web
{
    [ApiController]
    public abstract class Action : ControllerBase
    {
        private readonly Dictionary<ErrorType, int> statuses = new()
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
            if (!statuses.TryGetValue(error.Type, out var status))
            {
                status = 500;
            }

            if (error.Type != ErrorType.ValidationError)
            {
                return Problem(error.Message, null, status, null, null);
            }

            var errors = new Dictionary<string, string[]>();

            foreach (var err in error.Errors)
            {
                errors.Add(ToCamelCase(err.Key), new[] { err.Value });
            }

            var details = new ValidationProblemDetails(errors)
            {
                Detail = error.Message,
                Status = status,
            };

            return StatusCode(status, details);
        }

        private static string ToCamelCase(string err)
        {
            return err.Substring(0, 1).ToLower() + err.Substring(1);
        }
    }
}
