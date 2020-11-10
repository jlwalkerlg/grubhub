using Autofac;
using FoodSnap.Application.Menus;
using FoodSnap.Infrastructure.Persistence.Dapper.Repositories.Menus;

namespace FoodSnap.Web.ServiceRegistration
{
    public static class DtoRepositoriesRegistrar
    {
        public static void AddDtoRepositories(this ContainerBuilder builder)
        {
            builder
                .RegisterType<DapperMenuDtoRepository>()
                .As<IMenuDtoRepository>()
                .InstancePerDependency();
        }
    }
}
