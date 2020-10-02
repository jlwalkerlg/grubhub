using System;
using System.Collections.Generic;

namespace FoodSnap.Domain.Menus
{
    public class MenuCategory : Entity
    {
        private List<MenuItem> items = new List<MenuItem>();
        public IReadOnlyList<MenuItem> Items => items;

        public MenuCategory(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"{nameof(name)} can't be empty.");
            }

            Name = name;
        }

        public string Name { get; }

        internal void AddItem(string name, string description, Money price)
        {
            var item = new MenuItem(name, description, price);
            items.Add(item);
        }

        // EF Core
        private MenuCategory() { }
    }
}
