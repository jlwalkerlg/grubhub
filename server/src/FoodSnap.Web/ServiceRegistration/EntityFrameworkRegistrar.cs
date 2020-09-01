using FoodSnap.Application;
using FoodSnap.Application.Events;
using FoodSnap.Application.Restaurants;
using FoodSnap.Application.Users;
using FoodSnap.Infrastructure.Persistence.EF;
using FoodSnap.Infrastructure.Persistence.EF.Repositories;
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
            services.AddScoped<IRestaurantManagerRepository, EFRestaurantManagerRepository>();
            services.AddScoped<IRestaurantApplicationRepository, EFRestaurantApplicationRepository>();
            services.AddScoped<IRestaurantRepository, EFRestaurantRepository>();
            services.AddScoped<IEventRepository, EFEventRepository>();
        }
    }
}
