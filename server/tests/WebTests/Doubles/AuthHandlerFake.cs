using System;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Web.Domain.Users;

namespace WebTests.Doubles
{
    public class AuthHandlerFake : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public AuthHandlerFake(
            IHttpContextAccessor httpContextAccessor,
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock) : base(options, logger, encoder, clock)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var headers = httpContextAccessor.HttpContext.Request.Headers;

            if (headers.TryGetValue("X-USER-ID", out var id))
            {
                if (!headers.TryGetValue("X-USER-ROLE", out var role))
                {
                    role = UserRole.Customer.ToString();
                }

                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, id),
                    new Claim(ClaimTypes.Role, role),
                };
                var identity = new ClaimsIdentity(claims, "Test");
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, "Test");

                return Task.FromResult(AuthenticateResult.Success(ticket));
            }

            return Task.FromResult(AuthenticateResult.Fail("Unauthenticated."));
        }
    }
}
