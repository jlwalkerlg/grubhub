using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Web.Domain.Users;

namespace Web.Features.Restaurants.UpdateCuisines
{
    public class UpdateCuisinesAction : Action
    {
        private readonly ISender sender;

        public UpdateCuisinesAction(ISender sender)
        {
            this.sender = sender;
        }

        [Authorize(Roles = nameof(UserRole.RestaurantManager))]
        [HttpPut("/restaurants/{restaurantId:guid}/cuisines")]
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

            return result ? Ok() : Problem(result.Error);
        }
    }
}
