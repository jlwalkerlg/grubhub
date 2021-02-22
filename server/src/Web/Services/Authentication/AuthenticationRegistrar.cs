using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;

namespace Web.Services.Authentication
{
    public static class AuthenticationRegistrar
    {
        public static void AddAuth(this IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.Name = "auth_cookie";
                });

            services.AddScoped<IAuthenticator, Authenticator>();
        }
    }
}
