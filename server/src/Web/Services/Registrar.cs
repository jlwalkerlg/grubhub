using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Web.Features.Restaurants.SearchRestaurants;
using Web.Services.Validation;

namespace Web.Services
{
    public static class Registrar
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
    }
}
