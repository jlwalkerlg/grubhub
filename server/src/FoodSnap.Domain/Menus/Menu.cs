using System.Collections.Generic;
using System;
using System.Linq;

namespace FoodSnap.Domain.Menus
{
    public class Menu : Entity
    {
        private List<MenuCategory> categories = new List<MenuCategory>();
        public IReadOnlyList<MenuCategory> Categories => categories;

        public Menu(Guid restaurantId)
        {
            RestaurantId = restaurantId;
        }

        public Guid RestaurantId { get; }

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

        // EF Core
        private Menu() { }
    }
}
