using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Web.Features.Restaurants.UpdateRestaurantDetails
{
    public class UpdateRestaurantDetailsAction : Action
    {
        private readonly ISender sender;

        public UpdateRestaurantDetailsAction(ISender sender)
        {
            this.sender = sender;
        }

        [Authorize]
        [HttpPut("/restaurants/{id:guid}")]
        public async Task<IActionResult> Execute(
            [FromRoute] Guid id,
            [FromBody] UpdateRestaurantDetailsRequest request)
        {
            var command = new UpdateRestaurantDetailsCommand
            {
                RestaurantId = id,
                Name = request.Name,
                Description = request.Description,
                PhoneNumber = request.PhoneNumber,
                DeliveryFee = request.DeliveryFee,
                MinimumDeliverySpend = request.MinimumDeliverySpend,
                MaxDeliveryDistanceInKm = request.MaxDeliveryDistanceInKm,
                EstimatedDeliveryTimeInMinutes = request.EstimatedDeliveryTimeInMinutes,
            };

            var result = await sender.Send(command);

            return result ? Ok() : Problem(result.Error);
        }
    }
}
