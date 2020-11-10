using Autofac;
using FoodSnap.Application.Menus;
using FoodSnap.Application.Restaurants;
using FoodSnap.Infrastructure.Persistence.Dapper.Repositories.Menus;
using FoodSnap.Infrastructure.Persistence.Dapper.Repositories.Restaurants;

namespace FoodSnap.Web.ServiceRegistration
{
    public static class DtoRepositoriesRegistrar
    {
        public static void AddDtoRepositories(this ContainerBuilder builder)
        {
            builder
                .RegisterType<DPMenuDtoRepository>()
                .As<IMenuDtoRepository>()
                .InstancePerDependency();

            builder
                .RegisterType<DPRestaurantDtoRepository>()
                .As<IRestaurantDtoRepository>()
                .InstancePerDependency();
        }
    }
}
