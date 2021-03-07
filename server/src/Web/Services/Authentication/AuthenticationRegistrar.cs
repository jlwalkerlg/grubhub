using System.IO;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

                    options.Events.OnRedirectToLogin = async context =>
                    {
                        var problem = new ProblemDetails()
                        {
                            Detail = Error.Unauthorised().Message,
                            Status = 401,
                        };

                        context.Response.StatusCode = 401;

                        await context.Response.WriteAsJsonAsync(problem);
                        await context.Response.Body.FlushAsync();
                    };

                    options.Events.OnRedirectToAccessDenied = async context =>
                    {
                        var problem = new ProblemDetails()
                        {
                            Detail = Error.Unauthorised().Message,
                            Status = 403,
                        };

                        context.Response.StatusCode = 403;

                        await context.Response.WriteAsJsonAsync(problem);
                        await context.Response.Body.FlushAsync();
                    };
                });

            services.AddScoped<IAuthenticator, Authenticator>();
        }
    }
}
