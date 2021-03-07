using Microsoft.Extensions.DependencyInjection;

namespace Web.Services.Mail
{
    public static class MailRegistrar
    {
        public static void AddMail(this IServiceCollection services)
        {
            services.AddSingleton<IMailer, MailKitMailer>();
        }
    }
}
