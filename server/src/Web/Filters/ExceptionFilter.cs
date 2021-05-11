using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Web.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ExceptionFilter> logger;

        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            this.logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            logger.LogError(context.Exception, "Unhandled {Exception}: {Message}",
                context.Exception.GetType().FullName,
                context.Exception.Message);

            var details = new ProblemDetails
            {
                Detail = "Server error.",
                Status = 500,
            };

            context.Result = new ObjectResult(details)
            {
                StatusCode = details.Status,
            };

            context.ExceptionHandled = true;
        }
    }
}
