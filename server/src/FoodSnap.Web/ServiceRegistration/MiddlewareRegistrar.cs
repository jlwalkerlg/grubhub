using Autofac;
using FoodSnap.Application;
using FoodSnap.Application.Middleware;
using FoodSnap.Application.Validation;

namespace FoodSnap.Web.ServiceRegistration
{
    public static class MiddlewareRegistrar
    {
        public static void AddMiddleware(this ContainerBuilder builder)
        {
            builder
                .RegisterAssemblyTypes(typeof(IRequest).Assembly)
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
