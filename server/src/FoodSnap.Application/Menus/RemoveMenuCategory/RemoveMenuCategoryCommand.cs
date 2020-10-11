using System;
using FoodSnap.Application.Services.Authentication;

namespace FoodSnap.Application.Menus.RemoveMenuCategory
{
    [Authenticate]
    public class RemoveMenuCategoryCommand : IRequest
    {
        public Guid RestaurantId { get; set; }
        public string CategoryName { get; set; }
    }
}
