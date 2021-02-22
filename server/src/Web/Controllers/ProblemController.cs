using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class ProblemController : Controller
    {
        [HttpGet("/problem")]
        public IActionResult GetProblem()
        {
            var errors = new Dictionary<string, string[]>()
            {
                { "MenuItemId", new[] { "Required." } }
            };

            var details = new ValidationProblemDetails(errors)
            {
                Detail = "Invalid request.",
                Status = 422,
            };

            return StatusCode(422, details);
        }
    }
}
