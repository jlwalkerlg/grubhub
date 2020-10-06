using System;
using FoodSnap.Application.Services.Authentication;

namespace FoodSnap.Application.Menus.AddMenuCategory
{
    [Authenticate]
    public class AddMenuCategoryCommand : IRequest
    {
        public Guid MenuId { get; set; }
        public string Name { get; set; }
    }
}
