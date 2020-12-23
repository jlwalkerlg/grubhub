using Autofac;
using Application;
using Application.Services.Authentication;
using Application.Validation;

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
                .RegisterAssemblyTypes(typeof(IRequest).Assembly, typeof(Startup).Assembly)
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
