using System;
using System.Collections.Generic;
using System.Linq;

namespace Web.Domain.Menus
{
    public class MenuCategory : Entity<MenuCategory>
    {
        public string name;
        private readonly List<MenuItem> items = new();

        internal MenuCategory(Guid id, string name)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Id must not be empty.");
            }

            Id = id;
            Name = name;
        }

        public Guid Id { get; }

        public string Name
        {
            get => name;
            internal set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Name must be empty.");
                }

                name = value;
            }
        }

        public IReadOnlyList<MenuItem> Items => items;

        public MenuItem AddItem(Guid id, string name, string description, Money price)
        {
            if (ContainsItem(name))
            {
                throw new InvalidOperationException($"Item {name} already exists for this category.");
            }

            var item = new MenuItem(id, name, description, price);

            items.Add(item);

            return item;
        }

        public bool ContainsItem(Guid id)
        {
            return items.Any(x => x.Id == id);
        }

        public bool ContainsItem(string name)
        {
            return items.Any(x => x.Name == name);
        }

        public MenuItem GetItem(Guid id)
        {
            return items.Single(x => x.Id == id);
        }

        public MenuItem GetItem(string name)
        {
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

        public void RenameItem(Guid id, string newName)
        {
            var item = items.SingleOrDefault(x => x.Id == id);

            if (item == null)
            {
                throw new InvalidOperationException("Item not found.");
            }

            if (item.Name == newName)
            {
                return;
            }

            if (items.Any(x => x.Name == newName))
            {
                throw new InvalidOperationException("Item already exists.");
            }

            item.Name = newName;
        }

        public void RemoveItem(Guid id)
        {
            var item = items.SingleOrDefault(x => x.Id == id);

            if (item == null)
            {
                throw new InvalidOperationException("Item not found.");
            }

            items.Remove(item);
        }

        // EF Core
        private MenuCategory() { }
    }
}
