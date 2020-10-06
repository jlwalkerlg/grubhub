using System;

namespace FoodSnap.Application.Menus.RemoveMenuItem
{
    public class RemoveMenuItemCommand : IRequest
    {
        public Guid MenuId { get; set; }
        public string Category { get; set; }
        public string Item { get; set; }
    }
}
