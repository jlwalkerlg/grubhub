using System;

namespace FoodSnap.Application.Menus.AddMenuItem
{
    public class AddMenuItemCommand : IRequest
    {
        public Guid MenuId { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
