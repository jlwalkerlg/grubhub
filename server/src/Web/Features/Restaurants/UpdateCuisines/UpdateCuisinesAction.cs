using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Web.Features.Restaurants.UpdateCuisines
{
    public class UpdateCuisinesAction : Action
    {
        private readonly ISender sender;

        public UpdateCuisinesAction(ISender sender)
        {
            this.sender = sender;
        }

        [HttpPut("/restaurants/{restaurantId}/cuisines")]
        public async Task<IActionResult> Execute(
            [FromRoute] Guid restaurantId,
            [FromBody] UpdateCuisinesRequest request
        )
        {
            var command = new UpdateCuisinesCommand()
            {
                RestaurantId = restaurantId,
                Cuisines = request.Cuisines ?? new(),
            };

            var result = await sender.Send(command);

            return result ? Ok() : Error(result.Error);
        }
    }
}
