using Autofac;
using FoodSnap.Application;
using FoodSnap.Application.Services.Authentication;
using FoodSnap.Application.Validation;

namespace FoodSnap.Web.ServiceRegistration
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
