using System.Threading.Tasks;
using FoodSnap.Application;
using FoodSnap.Application.Restaurants.RegisterRestaurant;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FoodSnap.Web.Actions.Restaurants.RegisterRestaurant
{
    public class RegisterRestaurantAction : Action
    {
        private readonly IMediator mediator;
        private readonly IPresenter<RegisterRestaurantCommand, Result> presenter;

        public RegisterRestaurantAction(IMediator mediator, IPresenter<RegisterRestaurantCommand, Result> presenter)
        {
            this.mediator = mediator;
            this.presenter = presenter;
        }

        public async Task<IActionResult> Execute(RegisterRestaurantRequest request)
        {
            var command = new RegisterRestaurantCommand(
                request.ManagerName,
                request.ManagerEmail,
                request.ManagerPassword,
                request.RestaurantName,
                request.RestaurantPhoneNumber,
                request.AddressLine1,
                request.AddressLine2,
                request.Town,
                request.Postcode);

            var result = await mediator.Send(command);

            return presenter.Present(result);
        }
    }
}
