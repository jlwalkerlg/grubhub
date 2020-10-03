using System;

namespace FoodSnap.Domain.Menus
{
    public class MenuId : GuidId
    {
        public MenuId(Guid value) : base(value)
        {
        }
    }
}
