using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Web.Features.Restaurants.SearchRestaurants;
using Web.Services.Validation;
using Web.Workers;

namespace Web
{
    public static class ServiceRegistrar
    {
        public static void AddMediatR(this IServiceCollection services)
        {
            services.AddMediatR(typeof(Startup).Assembly);

            services.AddTransient(
                typeof(IPipelineBehavior<,>),
                typeof(ValidationMiddleware<,>));
        }

        public static void AddDapper(this IServiceCollection services)
        {
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

            services.AddScoped<IRestaurantSearcher, DPRestaurantSearcher>();
        }

        public static void AddEventWorker(this IServiceCollection services)
        {
            services.AddScoped<EventDispatcher>();
            services.AddHostedService<EventWorker>();
        }
    }
}
