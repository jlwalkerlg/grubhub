using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Web.Envelopes;

namespace Web.Filters
{
    public class ExceptionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
            {
                var envelope = new ErrorEnvelope("Server error.");

                context.Result = new ObjectResult(envelope)
                {
                    StatusCode = 500,
                };

                context.ExceptionHandled = true;
            }
        }
    }
}
