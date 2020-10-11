using System;

namespace FoodSnap.Application.Menus.RemoveMenuItem
{
    public class RemoveMenuItemCommand : IRequest
    {
        public Guid RestaurantId { get; set; }
        public string CategoryName { get; set; }
        public string ItemName { get; set; }
    }
}
