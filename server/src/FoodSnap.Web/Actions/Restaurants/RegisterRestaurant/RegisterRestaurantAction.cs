using System.Threading.Tasks;
using FoodSnap.Application.Restaurants.RegisterRestaurant;
using FoodSnap.Web.ErrorPresenters;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FoodSnap.Web.Actions.Restaurants.RegisterRestaurant
{
    public class RegisterRestaurantAction : Action
    {
        private readonly IMediator mediator;
        private readonly IErrorPresenterFactory errorPresenterFactory;

        public RegisterRestaurantAction(
            IMediator mediator,
            IErrorPresenterFactory errorPresenterFactory)
        {
            this.mediator = mediator;
            this.errorPresenterFactory = errorPresenterFactory;
        }

        [HttpPost("/restaurants/register")]
        public async Task<IActionResult> Execute(RegisterRestaurantCommand command)
        {
            var result = await mediator.Send(command);

            if (!result.IsSuccess)
            {
                return errorPresenterFactory
                    .Make(result.Error)
                    .Present(result.Error);
            }

            return StatusCode(201);
        }
    }
}
