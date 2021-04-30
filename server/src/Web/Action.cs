using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Web
{
    [ApiController]
    public abstract class Action : ControllerBase
    {
        private static readonly ProblemFactory ProblemFactory = new();

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
            return Problem(ProblemFactory.Make(error));
        }

        private IActionResult Problem(ProblemDetails details)
        {
            return details is ValidationProblemDetails validationProblemDetails
                ? ValidationProblem(validationProblemDetails)
                : Problem(details.Detail, details.Instance, details.Status, details.Title, details.Type);
        }
    }
}
