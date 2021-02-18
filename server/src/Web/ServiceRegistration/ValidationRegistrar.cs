using Autofac;
using Web.Services.Validation;

namespace Web.ServiceRegistration
{
    public static class ValidationRegistrar
    {
        public static void AddValidators(this ContainerBuilder builder)
        {
            builder
                .RegisterAssemblyTypes(typeof(Startup).Assembly)
                .AsClosedTypesOf(typeof(IValidator<>))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}
