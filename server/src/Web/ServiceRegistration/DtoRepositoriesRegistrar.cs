using Autofac;
using Web.Data.Dapper.Repositories.Restaurants;
using Web.Data.Dapper.Repositories.Users;

namespace Web.ServiceRegistration
{
    public static class DtoRepositoriesRegistrar
    {
        public static void AddDtoRepositories(this ContainerBuilder builder)
        {
            builder.RegisterType<DPRestaurantDtoRepository>()
                .AsImplementedInterfaces();

            builder.RegisterType<DPUserDtoRepository>()
                .AsImplementedInterfaces();
        }
    }
}
