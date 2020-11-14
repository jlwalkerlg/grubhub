using System;
using FoodSnap.Application.Services.Authentication;

namespace FoodSnap.Application.Menus.RemoveMenuCategory
{
    [Authenticate]
    public record RemoveMenuCategoryCommand : IRequest
    {
        public Guid RestaurantId { get; init; }
        public string CategoryName { get; init; }
    }
}
