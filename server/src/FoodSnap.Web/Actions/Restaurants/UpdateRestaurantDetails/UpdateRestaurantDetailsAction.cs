using System;
using System.Threading.Tasks;
using FoodSnap.Application.Restaurants.UpdateRestaurantDetails;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FoodSnap.Web.Actions.Restaurants.UpdateRestaurantDetails
{
    public class UpdateRestaurantDetailsAction : Action
    {
        private readonly IMediator mediator;

        public UpdateRestaurantDetailsAction(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPut("/restaurants/{id}")]
        public async Task<IActionResult> Execute(
            [FromRoute] Guid id,
            [FromBody] UpdateRestaurantDetailsRequest request)
        {
            var command = new UpdateRestaurantDetailsCommand
            {
                Id = id,
                Name = request.Name,
                PhoneNumber = request.PhoneNumber,
            };

            var result = await mediator.Send(command);

            if (!result.IsSuccess)
            {
                return PresentError(result.Error);
            }

            return Ok();
        }
    }
}
