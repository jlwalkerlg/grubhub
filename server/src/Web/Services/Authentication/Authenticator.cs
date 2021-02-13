using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Web.Domain.Users;

namespace Web.Services.Authentication
{
    public class Authenticator : IAuthenticator
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public Authenticator(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        private HttpContext HttpContext => httpContextAccessor.HttpContext;

        public bool IsAuthenticated => HttpContext.User.Identity.IsAuthenticated;

        public UserId UserId => IsAuthenticated
            ? new UserId(Guid.Parse(HttpContext.User.Identity.Name))
            : null;

        public async Task SignIn(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Id.Value.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
            };

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            var properties = new AuthenticationProperties()
            {
                ExpiresUtc = DateTime.Now.AddDays(14),
            };

            await HttpContext
                .SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal,
                    properties
                );
        }

        public async Task SignOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
