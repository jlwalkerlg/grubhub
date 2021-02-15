using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Web.Envelopes;

namespace Web.Services.Antiforgery
{
    public class AntiforgeryFilter : Attribute, IAsyncResourceFilter
    {
        private readonly IAntiforgery antiforgery;

        public AntiforgeryFilter(IAntiforgery antiforgery)
        {
            this.antiforgery = antiforgery;
        }

        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            if (await IsRequestValidAsync(context))
            {
                await next();
                return;
            }

            var envelope = new ErrorEnvelope("Antiforgery token mismatch.");

            context.Result = new ObjectResult(envelope)
            {
                StatusCode = 419,
            };
        }

        private async Task<bool> IsRequestValidAsync(ResourceExecutingContext context)
        {
            return HasIgnoreAttribute(context)
                || await antiforgery.IsRequestValidAsync(context.HttpContext);
        }

        private bool HasIgnoreAttribute(ResourceExecutingContext context)
        {
            if (context.ActionDescriptor is ControllerActionDescriptor descriptor)
            {
                var attrs = descriptor.MethodInfo.GetCustomAttributes(inherit: true);
                return attrs.Any(x => x is IgnoreAntiforgeryValidationAttribute);
            }

            return false;
        }
    }
}
