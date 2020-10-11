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
        public IReadOnlyList<MenuCategory> Categories => categories;

        public void AddCategory(string name)
        {
            if (ContainsCategory(name))
            {
                throw new InvalidOperationException($"Category {name} already exists.");
            }

            categories.Add(new MenuCategory(name));
        }

        public bool ContainsCategory(string name)
        {
            return categories.Any(x => x.Name == name);
        }

        public MenuCategory GetCategory(string name)
        {
            // TODO: throw exception or return null if doesn't exist?
            return categories.Single(x => x.Name == name);
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
