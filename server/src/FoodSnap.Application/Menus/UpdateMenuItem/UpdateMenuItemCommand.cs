using System;
using FoodSnap.Application.Services.Authentication;

namespace FoodSnap.Application.Menus.UpdateMenuItem
{
    [Authenticate]
    public class UpdateMenuItemCommand : IRequest
    {
        public Guid MenuId { get; set; }
        public string Category { get; set; }
        public string Item { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
