using System.Threading.Tasks;
using Web.Features.Restaurants.RegisterRestaurant;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web.Features.Restaurants.RegisterRestaurant
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
                return Error(result.Error);
            }

            return StatusCode(201);
        }
    }
}
