using FoodSnap.Application;
using FoodSnap.Infrastructure.Persistence.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FoodSnap.Web.ServiceRegistration
{
    public static class EntityFrameworkRegistrar
    {
        public static void AddEntityFramework(
            this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<AppDbContext>(
                options => options.UseNpgsql(config["DbConnectionString"]));

            services.AddScoped<IUnitOfWork, EFUnitOfWork>();
        }
    }
}
