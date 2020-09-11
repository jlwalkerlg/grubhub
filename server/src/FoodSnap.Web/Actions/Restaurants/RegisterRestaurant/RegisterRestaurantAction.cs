using System.Threading.Tasks;
using FoodSnap.Application.Restaurants.RegisterRestaurant;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FoodSnap.Web.Actions.Restaurants.RegisterRestaurant
{
    public class RegisterRestaurantAction : Action
    {
        private readonly IMediator mediator;

        public RegisterRestaurantAction(
            IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("/restaurants/register")]
        public async Task<IActionResult> Execute(RegisterRestaurantCommand command)
        {
            var result = await mediator.Send(command);

            if (!result.IsSuccess)
            {
                return PresentError(result.Error);
            }

            return StatusCode(201);
        }
    }
}
