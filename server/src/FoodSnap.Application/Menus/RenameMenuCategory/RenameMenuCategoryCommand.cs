using System;
using FoodSnap.Application.Services.Authentication;

namespace FoodSnap.Application.Menus.RenameMenuCategory
{
    [Authenticate]
    public class RenameMenuCategoryCommand : IRequest
    {
        public Guid RestaurantId { get; set; }
        public string OldName { get; set; }
        public string NewName { get; set; }
    }
}
