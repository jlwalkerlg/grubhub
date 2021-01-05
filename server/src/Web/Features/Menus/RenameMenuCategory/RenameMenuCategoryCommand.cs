using System;
using Web.Services.Authentication;

namespace Web.Features.Menus.RenameMenuCategory
{
    [Authenticate]
    public record RenameMenuCategoryCommand : IRequest
    {
        public Guid RestaurantId { get; init; }
        public string OldName { get; init; }
        public string NewName { get; init; }
    }
}
