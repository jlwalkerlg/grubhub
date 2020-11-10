using Autofac;
using FoodSnap.Infrastructure.Persistence.Dapper.Repositories.Menus;

namespace FoodSnap.Web.ServiceRegistration
{
    public static class DtoRepositoriesRegistrar
    {
        public static void AddDtoRepositories(this ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(DPMenuDtoRepository).Assembly)
                .Where(x => x.Name.EndsWith("DtoRepository"))
                .AsImplementedInterfaces()
                .InstancePerDependency();
        }
    }
}
