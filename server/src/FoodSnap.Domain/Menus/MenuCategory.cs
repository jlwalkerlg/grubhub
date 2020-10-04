using System;
using System.Collections.Generic;

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

        public Guid Id { get; } = Guid.NewGuid();

        private List<MenuItem> items = new List<MenuItem>();
        public IReadOnlyList<MenuItem> Items => items;

        public string Name { get; }

        internal void AddItem(string name, string description, Money price)
        {
            var item = new MenuItem(name, description, price);
            items.Add(item);
        }

        protected override bool IdentityEquals(MenuCategory other)
        {
            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        // EF Core
        private MenuCategory() { }
    }
}
