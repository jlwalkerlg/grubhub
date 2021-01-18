using System;
using System.Collections.Generic;
using System.Linq;
using Web.Domain.Restaurants;

namespace Web.Domain.Menus
{
    public class Menu : Entity<Menu>
    {
        private readonly List<MenuCategory> categories = new();

        public Menu(RestaurantId restaurantId)
        {
            if (restaurantId == null)
            {
                throw new ArgumentNullException(nameof(restaurantId));
            }

            RestaurantId = restaurantId;
        }

        public RestaurantId RestaurantId { get; }

        public IReadOnlyList<MenuCategory> Categories => categories;

        public MenuCategory AddCategory(Guid id, string name)
        {
            if (ContainsCategory(name))
            {
                throw new InvalidOperationException($"Category {name} already exists.");
            }

            var category = new MenuCategory(id, name);

            categories.Add(category);

            return category;
        }

        public void RemoveCategory(Guid id)
        {
            if (!ContainsCategoryById(id))
            {
                throw new InvalidOperationException("Category doesn't exist.");
            }

            var category = GetCategoryById(id);

            categories.Remove(category);
        }

        public void RenameCategory(Guid id, string newName)
        {
            if (!ContainsCategoryById(id))
            {
                throw new InvalidOperationException("Category doesn't exist.");
            }

            var category = GetCategoryById(id);

            if (category.Name != newName && ContainsCategory(newName))
            {
                throw new InvalidOperationException("Category already exists.");
            }

            category.Name = newName;
        }

        public bool ContainsCategoryById(Guid id)
        {
            return categories.Any(x => x.Id == id);
        }

        // TODO: rename
        public bool ContainsCategory(string name)
        {
            return categories.Any(x => x.Name == name);
        }

        public MenuCategory GetCategoryById(Guid id)
        {
            return categories.Single(x => x.Id == id);
        }

        // TODO: rename
        public MenuCategory GetCategory(Guid id)
        {
            return categories.Single(x => x.Id == id);
        }

        public MenuCategory GetCategory(string name)
        {
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
