using System;
using FoodSnap.Application.Services.Authentication;

namespace FoodSnap.Application.Menus.RenameMenuCategory
{
    [Authenticate]
    public record RenameMenuCategoryCommand : IRequest
    {
        public Guid RestaurantId { get; init; }
        public string OldName { get; init; }
        public string NewName { get; init; }
    }
}
