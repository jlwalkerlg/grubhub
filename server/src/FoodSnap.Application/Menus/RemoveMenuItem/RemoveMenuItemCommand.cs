using System;

namespace FoodSnap.Application.Menus.RemoveMenuItem
{
    public class RemoveMenuItemCommand : IRequest
    {
        public Guid MenuId { get; set; }
        public string CategoryName { get; set; }
        public string ItemName { get; set; }
    }
}
