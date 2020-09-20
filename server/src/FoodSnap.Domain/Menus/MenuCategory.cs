using System;
using System.Collections.Generic;

namespace FoodSnap.Domain.Menus
{
    public class MenuCategory : Entity
    {
        private List<MenuItem> items = new List<MenuItem>();

        public MenuCategory(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"{nameof(name)} can't be empty.");
            }

            Name = name;
        }

        public string Name { get; }

        public void AddItem(string name, string description, Money price)
        {
            items.Add(new MenuItem(name, description, price));
        }

        // EF Core
        private MenuCategory() { }
    }
}
