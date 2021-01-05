using Autofac;
using Web.Data.Dapper.Repositories.Menus;

namespace Web.ServiceRegistration
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
