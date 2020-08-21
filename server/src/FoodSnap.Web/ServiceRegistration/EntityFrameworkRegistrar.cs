using FoodSnap.Application;
using FoodSnap.Infrastructure.Persistence.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FoodSnap.Web.ServiceRegistration
{
    public static class EntityFrameworkRegistrar
    {
        public static void AddEntityFramework(this IServiceCollection services)
        {
            // TODO: real configuration string
            services.AddDbContext<AppDbContext>(
                options => options.UseNpgsql(""));

            services.AddScoped<IUnitOfWork, EFUnitOfWork>();
        }
    }
}
