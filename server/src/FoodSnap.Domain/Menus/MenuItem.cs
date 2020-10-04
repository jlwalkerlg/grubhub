using System;

namespace FoodSnap.Domain.Menus
{
    public class MenuItem : Entity<MenuItem>
    {
        public MenuItem(string name, string description, Money price)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"{nameof(name)} can't be empty.");
            }

            if (price == null)
            {
                throw new ArgumentNullException(nameof(price));
            }

            Name = name;
            Description = description;
            Price = price;
        }

        public string Name { get; }
        public string Description { get; }
        public Money Price { get; }

        protected override bool IdentityEquals(MenuItem other)
        {
            return Name == other.Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        // EF Core
        private MenuItem() { }
    }
}
