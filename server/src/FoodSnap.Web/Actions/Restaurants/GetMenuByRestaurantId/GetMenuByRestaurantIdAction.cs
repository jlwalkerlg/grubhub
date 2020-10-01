using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FoodSnap.Web.Envelopes;
using FoodSnap.Web.Queries.Restaurants;
using FoodSnap.Web.Queries.Restaurants.GetMenuByRestaurantId;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FoodSnap.Web.Actions.Restaurants.GetMenuByRestaurantId
{
    public class GetMenuByRestaurantIdAction : Action
    {
        private readonly IMediator mediator;

        public GetMenuByRestaurantIdAction(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("/restaurants/{restaurantId}/menu")]
        public async Task<IActionResult> Execute([FromRoute] Guid restaurantId)
        {
            await Task.CompletedTask;

            var query = new GetMenuByRestaurantIdQuery
            {
                RestaurantId = restaurantId,
            };

            var result = await mediator.Send(query);

            if (!result.IsSuccess)
            {
                return PresentError(result.Error);
            }

            return Ok(new DataEnvelope
            {
                Data = result.Value
            });

            // var menuId = Guid.NewGuid();
            // var pizzaCategoryId = Guid.NewGuid();

            // return Ok(new DataEnvelope
            // {
            //     Data = new MenuDto
            //     {
            //         Id = menuId,
            //         RestaurantId = Guid.NewGuid(),
            //         Name = "Chow Main",
            //         Categories = new List<MenuCategoryDto>
            //         {
            //             new MenuCategoryDto
            //             {
            //                 Id = pizzaCategoryId,
            //                 MenuId = menuId,
            //                 Name = "Pizza",
            //                 Items = new List<MenuItemDto>
            //                 {
            //                     new MenuItemDto
            //                     {
            //                         Id = Guid.NewGuid(),
            //                         MenuCategoryId = pizzaCategoryId,
            //                         Name = "Margherita",
            //                         Description = "Cheese & tomato",
            //                         Price = 9.99m,
            //                     }
            //                 }
            //             }
            //         }
            //     }
            // });
        }
    }
}
