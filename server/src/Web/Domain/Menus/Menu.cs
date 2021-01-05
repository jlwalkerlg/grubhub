using System;
using System.Collections.Generic;
using System.Linq;
using Web.Domain.Restaurants;

namespace Web.Domain.Menus
{
    public class Menu : Entity<Menu>
    {
        public Menu(RestaurantId restaurantId)
        {
            if (restaurantId == null)
            {
                throw new ArgumentNullException(nameof(restaurantId));
            }

            RestaurantId = restaurantId;
        }

        public RestaurantId RestaurantId { get; }

        private readonly List<MenuCategory> categories = new();
        public IReadOnlyList<MenuCategory> Categories => categories;

        public void AddCategory(string name)
        {
            if (ContainsCategory(name))
            {
                throw new InvalidOperationException($"Category {name} already exists.");
            }

            categories.Add(new MenuCategory(name));
        }

        public void RemoveCategory(string name)
        {
            if (!ContainsCategory(name))
            {
                throw new InvalidOperationException($"Category {name} doesn't exist.");
            }

            var category = GetCategory(name);

            categories.Remove(category);
        }

        public void RenameCategory(string oldName, string newName)
        {
            if (!ContainsCategory(oldName))
            {
                throw new InvalidOperationException($"Category {oldName} doesn't exist.");
            }

            if (oldName == newName)
            {
                return;
            }

            var category = GetCategory(oldName);

            category.Name = newName;
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
            return RestaurantId == other.RestaurantId;
        }

        public override int GetHashCode()
        {
            return RestaurantId.GetHashCode();
        }

        // EF Core
        private Menu() { }
    }
}
