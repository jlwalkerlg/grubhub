using System;
using FoodSnap.Application.Services.Authentication;

namespace FoodSnap.Application.Menus.UpdateMenuItem
{
    [Authenticate]
    public class UpdateMenuItemCommand : IRequest
    {
        public Guid MenuId { get; set; }
        public string CategoryName { get; set; }
        public string OldItemName { get; set; }
        public string NewItemName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
