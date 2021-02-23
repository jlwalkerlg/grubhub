using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Web.Services.Authentication
{
    public static class AuthenticationRegistrar
    {
        public static void AddAuth(this IServiceCollection services, IHostEnvironment env)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.Name = "auth_cookie";
                    options.Cookie.SecurePolicy = env.IsProduction()
                        ? CookieSecurePolicy.Always
                        : CookieSecurePolicy.None;
                });

            services.AddScoped<IAuthenticator, Authenticator>();
        }
    }
}
