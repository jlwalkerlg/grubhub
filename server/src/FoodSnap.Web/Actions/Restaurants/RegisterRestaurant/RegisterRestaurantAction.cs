using System.Threading.Tasks;
using FoodSnap.Application.Restaurants.RegisterRestaurant;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FoodSnap.Web.Actions.Restaurants.RegisterRestaurant
{
    public class RegisterRestaurantAction : Action
    {
        private readonly ISender sender;

        public RegisterRestaurantAction(
            ISender sender)
        {
            this.sender = sender;
        }

        [HttpPost("/restaurants/register")]
        public async Task<IActionResult> Execute(RegisterRestaurantCommand command)
        {
            var result = await sender.Send(command);

            if (!result.IsSuccess)
            {
                return PresentError(result.Error);
            }

            return StatusCode(201);
        }
    }
}
