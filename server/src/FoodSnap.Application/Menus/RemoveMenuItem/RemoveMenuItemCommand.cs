using System;
using FoodSnap.Application.Services.Authentication;

namespace FoodSnap.Application.Menus.RemoveMenuItem
{
    [Authenticate]
    public class RemoveMenuItemCommand : IRequest
    {
        public Guid RestaurantId { get; set; }
        public string CategoryName { get; set; }
        public string ItemName { get; set; }
    }
}
