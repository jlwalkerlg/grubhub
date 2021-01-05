using System;
using System.Threading.Tasks;
using Web.Features.Menus.GetMenuByRestaurantId;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web.Features.Menus.GetMenuByRestaurantId
{
    public class GetMenuByRestaurantIdAction : Action
    {
        private readonly ISender sender;

        public GetMenuByRestaurantIdAction(ISender sender)
        {
            this.sender = sender;
        }

        [HttpGet("/restaurants/{restaurantId}/menu")]
        public async Task<IActionResult> Execute([FromRoute] Guid restaurantId)
        {
            var query = new GetMenuByRestaurantIdQuery
            {
                RestaurantId = restaurantId,
            };

            var result = await sender.Send(query);

            if (!result.IsSuccess)
            {
                return Error(result.Error);
            }

            return Ok(result.Value);
        }
    }
}
