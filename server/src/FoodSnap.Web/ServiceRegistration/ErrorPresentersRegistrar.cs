using Autofac;
using FoodSnap.Web.ErrorPresenters;

namespace FoodSnap.Web.ServiceRegistration
{
    public static class ErrorPresentersRegistrar
    {
        public static void AddErrorPresenters(this ContainerBuilder builder)
        {
            builder.RegisterType<ErrorPresenterFactory>()
                .As<IErrorPresenterFactory>()
                .InstancePerLifetimeScope();
        }
    }
}
