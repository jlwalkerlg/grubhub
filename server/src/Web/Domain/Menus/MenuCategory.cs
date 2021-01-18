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

        private MenuCategory() { } // EF Core

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

        public MenuItem GetItemById(Guid id)
        {
            return items.SingleOrDefault(x => x.Id == id);
        }

        public Result<MenuItem> AddItem(Guid id, string name, string description, Money price)
        {
            if (items.Any(x => x.Name == name))
            {
                return Error.BadRequest("Item already exists.");
            }

            var item = new MenuItem(id, name, description, price);

            items.Add(item);

            return Result.Ok(item);
        }

        public Result RenameItem(Guid id, string newName)
        {
            var item = items.SingleOrDefault(x => x.Id == id);

            if (item == null)
            {
                return Error.NotFound("Item not found.");
            }

            if (newName != item.Name && items.Any(x => x.Name == newName))
            {
                return Error.BadRequest("Item already exists.");
            }

            item.Name = newName;

            return Result.Ok();
        }

        public Result RemoveItem(Guid id)
        {
            var item = items.SingleOrDefault(x => x.Id == id);

            if (item == null)
            {
                return Error.NotFound("Item not found.");
            }

            items.Remove(item);

            return Result.Ok();
        }

        protected override bool IdentityEquals(MenuCategory other)
        {
            return Name == other.Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}
