using System;
using System.Threading.Tasks;
using FoodSnap.Application.Menus.GetMenuByRestaurantId;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FoodSnap.Web.Actions.Menus.GetMenuByRestaurantId
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
