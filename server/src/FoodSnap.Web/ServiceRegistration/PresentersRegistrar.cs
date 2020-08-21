using Autofac;
using FoodSnap.Application;
using FoodSnap.Application.Restaurants.RegisterRestaurant;
using FoodSnap.Web.Actions;
using FoodSnap.Web.Actions.Restaurants.RegisterRestaurant;

namespace FoodSnap.Web.ServiceRegistration
{
    public static class PresentersRegistrar
    {
        public static void AddPresenters(this ContainerBuilder builder)
        {
            builder.RegisterType<RegisterRestaurantPresenter>()
                .As<IPresenter<RegisterRestaurantCommand, Result>>()
                .InstancePerLifetimeScope();
        }
    }
}