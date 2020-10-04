using System.Collections.Generic;
using System;
using System.Linq;
using FoodSnap.Domain.Restaurants;

namespace FoodSnap.Domain.Menus
{
    public class Menu : Entity<Menu>
    {
        public Menu(MenuId id, RestaurantId restaurantId)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (restaurantId == null)
            {
                throw new ArgumentNullException(nameof(restaurantId));
            }

            Id = id;
            RestaurantId = restaurantId;
        }

        public MenuId Id { get; }

        public RestaurantId RestaurantId { get; }

        private List<MenuCategory> categories = new List<MenuCategory>();

        public Result AddCategory(string name)
        {
            if (categories.Any(x => x.Name == name))
            {
                return Result.Fail(Error.BadRequest($"Category {name} already exists."));
            }

            categories.Add(new MenuCategory(name));

            return Result.Ok();
        }

        public Result AddItem(string categoryName, string itemName, string itemDescription, Money price)
        {
            var category = categories.FirstOrDefault(x => x.Name == categoryName);

            if (category == null)
            {
                return Result.Fail(Error.BadRequest($"Category {categoryName} doesn't exist."));
            }

            return category.AddItem(itemName, itemDescription, price);
        }

        protected override bool IdentityEquals(Menu other)
        {
            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        // EF Core
        private Menu() { }
    }
}
