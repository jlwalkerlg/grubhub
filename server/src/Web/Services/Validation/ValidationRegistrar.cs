using Autofac;

namespace Web.Services.Validation
{
    public static class ValidationRegistrar
    {
        public static void AddValidators(this ContainerBuilder builder)
        {
            builder
                .RegisterAssemblyTypes(typeof(ValidationRegistrar).Assembly)
                .AsClosedTypesOf(typeof(IValidator<>))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}
