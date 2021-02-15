using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Web.Envelopes;

namespace Web.Middleware
{
    public class AntiforgeryMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IAntiforgery antiforgery;

        private readonly List<string> methodWhitelist = new()
        {
            "GET",
            "HEAD",
            "OPTIONS",
            "TRACE",
        };

        private readonly List<string> pathWhitelist = new()
        {
            "/auth/login",
            "/auth/logout",
            "/restaurants/register",
            "/stripe/webhooks",
            "/stripe/connect/webhooks",
        };

        public AntiforgeryMiddleware(RequestDelegate next, IAntiforgery antiforgery)
        {
            this.next = next;
            this.antiforgery = antiforgery;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!methodWhitelist.Contains(context.Request.Method)
                && !pathWhitelist.Contains(context.Request.Path)
                && !(await antiforgery.IsRequestValidAsync(context)))
            {
                context.Response.Body = new MemoryStream();

                var envelope = new ErrorEnvelope("Antiforgery token mismatch.");

                await JsonSerializer.SerializeAsync(context.Response.Body, envelope);
                context.Response.StatusCode = 419;

                return;
            }

            await next(context);
        }
    }
}
