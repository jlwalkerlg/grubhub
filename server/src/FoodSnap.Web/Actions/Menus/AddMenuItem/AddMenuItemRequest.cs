namespace FoodSnap.Web.Actions.Menus.AddMenuItem
{
    public class AddMenuItemRequest
    {
        public string CategoryName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
