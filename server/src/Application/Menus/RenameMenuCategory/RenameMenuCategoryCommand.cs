using System;
using Application.Services.Authentication;

namespace Application.Menus.RenameMenuCategory
{
    [Authenticate]
    public record RenameMenuCategoryCommand : IRequest
    {
        public Guid RestaurantId { get; init; }
        public string OldName { get; init; }
        public string NewName { get; init; }
    }
}
