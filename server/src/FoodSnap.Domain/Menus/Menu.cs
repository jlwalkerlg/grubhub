using System.Collections.Generic;
using System;
using System.Linq;
using FoodSnap.Domain.Restaurants;

namespace FoodSnap.Domain.Menus
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

        public Guid Id { get; } = Guid.NewGuid();

        private List<MenuCategory> categories = new List<MenuCategory>();
        public IReadOnlyList<MenuCategory> Categories => categories;

        public RestaurantId RestaurantId { get; }

        public void AddCategory(string categoryName)
        {
            if (categories.Any(x => x.Name == categoryName))
            {
                throw new InvalidOperationException("Category already exists.");
            }

            categories.Add(new MenuCategory(categoryName));
        }

        public void AddItem(Guid categoryId, string name, string description, Money price)
        {
            var category = categories.FirstOrDefault(x => x.Id == categoryId);

            if (category == null)
            {
                throw new InvalidOperationException($"Category doesn't exist.");
            }

            category.AddItem(name, description, price);
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
