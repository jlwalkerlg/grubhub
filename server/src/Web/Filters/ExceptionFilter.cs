using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Web.Envelopes;

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
            logger.LogCritical(context.Exception.ToString());

            var envelope = new ErrorEnvelope("Server error.");

            context.Result = new ObjectResult(envelope)
            {
                StatusCode = 500,
            };

            context.ExceptionHandled = true;
        }
    }
}
