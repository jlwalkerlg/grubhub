using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Web.Services.Authentication
{
    public class AuthenticationTestMiddleware
    {
        private readonly RequestDelegate next;

        public AuthenticationTestMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Headers.ContainsKey("user-id"))
            {
                var userId = context.Request.Headers["user-id"].ToString();

                var claims = new[]
                {
                        new Claim(ClaimTypes.Name, userId),
                    };

                var identity = new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(identity);

                var properties = new AuthenticationProperties()
                {
                    ExpiresUtc = DateTime.Now.AddDays(14),
                };

                context.User = principal;
            }

            await next(context);
        }
    }
}
