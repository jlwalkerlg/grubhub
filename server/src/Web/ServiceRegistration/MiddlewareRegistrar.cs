using Autofac;
using Web.Services.Authentication;
using Web.Services.Validation;

namespace Web.ServiceRegistration
{
    public static class MiddlewareRegistrar
    {
        public static void AddMiddleware(this ContainerBuilder builder)
        {
            builder
                .RegisterGeneric(typeof(AuthenticationMiddleware<,>))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder
                .RegisterAssemblyTypes(typeof(Startup).Assembly)
                .AsClosedTypesOf(typeof(IValidator<>))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder
                .RegisterGeneric(typeof(ValidationMiddleware<,>))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}