using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Web.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ExceptionFilter> logger;
        private readonly IHostEnvironment env;

        public ExceptionFilter(ILogger<ExceptionFilter> logger, IHostEnvironment env)
        {
            this.logger = logger;
            this.env = env;
        }

        public void OnException(ExceptionContext context)
        {
            logger.LogCritical(context.Exception.ToString());

            var details = new ProblemDetails()
            {
                Detail = env.IsProduction() ? "Server error." : context.Exception.ToString(),
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
