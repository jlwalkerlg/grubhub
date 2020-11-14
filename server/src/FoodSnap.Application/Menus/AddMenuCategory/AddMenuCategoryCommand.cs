using System;
using FoodSnap.Application.Services.Authentication;

namespace FoodSnap.Application.Menus.AddMenuCategory
{
    [Authenticate]
    public record AddMenuCategoryCommand : IRequest
    {
        public Guid RestaurantId { get; init; }
        public string Name { get; init; }
    }
}
