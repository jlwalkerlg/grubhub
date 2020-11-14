using System;
using FoodSnap.Application.Services.Authentication;

namespace FoodSnap.Application.Menus.RemoveMenuItem
{
    [Authenticate]
    public record RemoveMenuItemCommand : IRequest
    {
        public Guid RestaurantId { get; init; }
        public string CategoryName { get; init; }
        public string ItemName { get; init; }
    }
}
