using System;
using System.Collections.Generic;
using System.Linq;

namespace FoodSnap.Domain.Menus
{
    public class MenuCategory : Entity<MenuCategory>
    {
        internal MenuCategory(string name)
        {
            Name = name;
        }

        public string name;
        public string Name
        {
            get => name;
            internal set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException($"{nameof(name)} can't be empty.");
                }

                name = value;
            }
        }

        private List<MenuItem> items = new List<MenuItem>();
        public IReadOnlyList<MenuItem> Items => items;

        public void AddItem(string name, string description, Money price)
        {
            if (ContainsItem(name))
            {
                throw new InvalidOperationException($"Item {name} already exists for this category.");
            }

            items.Add(new MenuItem(name, description, price));
        }

        public bool ContainsItem(string name)
        {
            return items.Any(x => x.Name == name);
        }

        public MenuItem GetItem(string name)
        {
            // TODO: throw exception or return null if doesn't exist?
            return items.Single(x => x.Name == name);
        }

        protected override bool IdentityEquals(MenuCategory other)
        {
            return Name == other.Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public void RenameItem(string oldName, string newName)
        {
            var item = items.SingleOrDefault(x => x.Name == oldName);

            if (item == null)
            {
                throw new InvalidOperationException($"Item {oldName} not found for this category.");
            }

            if (oldName == newName)
            {
                return;
            }

            if (items.Any(x => x.Name == newName))
            {
                throw new InvalidOperationException($"Item {newName} already exists for this category.");
            }

            item.Name = newName;
        }

        public void RemoveItem(string name)
        {
            var item = items.SingleOrDefault(x => x.Name == name);

            if (item == null)
            {
                throw new InvalidOperationException($"Item {name} not found for this category.");
            }

            items.Remove(item);
        }

        // EF Core
        private MenuCategory() { }

    }
}
