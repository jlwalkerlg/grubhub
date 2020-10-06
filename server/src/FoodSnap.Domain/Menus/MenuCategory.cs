using System;
using System.Collections.Generic;
using System.Linq;

namespace FoodSnap.Domain.Menus
{
    public class MenuCategory : Entity<MenuCategory>
    {
        public MenuCategory(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"{nameof(name)} can't be empty.");
            }

            Name = name;
        }

        private List<MenuItem> items = new List<MenuItem>();
        public IReadOnlyList<MenuItem> Items => items;

        public string Name { get; }

        public void AddItem(string name, string description, Money price)
        {
            if (items.Any(x => x.Name == name))
            {
                throw new InvalidOperationException($"Item {name} already exists for this category.");
            }

            items.Add(new MenuItem(name, description, price));
        }

        protected override bool IdentityEquals(MenuCategory other)
        {
            return Name == other.Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        // EF Core
        private MenuCategory() { }
    }
}
