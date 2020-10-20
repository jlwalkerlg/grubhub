namespace FoodSnap.Web.Actions.Menus.UpdateMenuItem
{
    public class UpdateMenuItemRequest
    {
        public string NewItemName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
