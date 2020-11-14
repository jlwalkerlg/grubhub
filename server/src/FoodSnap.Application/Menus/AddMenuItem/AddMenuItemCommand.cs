using System;
using FoodSnap.Application.Services.Authentication;

namespace FoodSnap.Application.Menus.AddMenuItem
{
    [Authenticate]
    public record AddMenuItemCommand : IRequest
    {
        public Guid RestaurantId { get; init; }
        public string CategoryName { get; init; }
        public string ItemName { get; init; }
        public string Description { get; init; }
        public decimal Price { get; init; }
    }
}
