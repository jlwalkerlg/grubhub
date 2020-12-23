using System;
using Application.Services.Authentication;

namespace Application.Menus.UpdateMenuItem
{
    [Authenticate]
    public record UpdateMenuItemCommand : IRequest
    {
        public Guid RestaurantId { get; init; }
        public string CategoryName { get; init; }
        public string OldItemName { get; init; }
        public string NewItemName { get; init; }
        public string Description { get; init; }
        public decimal Price { get; init; }
    }
}
