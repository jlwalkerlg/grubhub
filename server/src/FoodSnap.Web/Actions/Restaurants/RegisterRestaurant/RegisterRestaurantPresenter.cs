using FoodSnap.Application;
using FoodSnap.Application.Restaurants.RegisterRestaurant;
using FoodSnap.Web.ErrorPresenters;
using Microsoft.AspNetCore.Mvc;

namespace FoodSnap.Web.Actions.Restaurants.RegisterRestaurant
{
    public class RegisterRestaurantPresenter : Presenter<RegisterRestaurantCommand, Result>
    {
        public RegisterRestaurantPresenter(IErrorPresenter errorPresenter) : base(errorPresenter)
        {
        }

        protected override IActionResult PresentSuccess(Result response)
        {
            return new StatusCodeResult(201);
        }
    }
}
