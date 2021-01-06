using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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

            if (!result)
            {
                return Error(result.Error);
            }

            return StatusCode(201);
        }
    }
}
