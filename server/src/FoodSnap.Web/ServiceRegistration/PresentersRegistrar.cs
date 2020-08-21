using System.Linq;
using System.Reflection;
using Autofac;
using FoodSnap.Web.Actions;

namespace FoodSnap.Web.ServiceRegistration
{
    public static class PresentersRegistrar
    {
        public static void AddPresenters(this ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(x =>
                    x.GetInterfaces().Any(i =>
                        i.IsGenericType
                        && i.GetGenericTypeDefinition() == typeof(IPresenter<,>)))
                .AsClosedTypesOf(typeof(IPresenter<,>))
                .SingleInstance();
        }
    }
}
