using System;

namespace FoodSnap.Domain.Menus
{
    public class MenuItem : Entity<MenuItem>
    {
        public MenuItem(string name, string description, Money price)
        {
            Name = name;
            Description = description;
            Price = price;
        }

        private string name;
        public string Name
        {
            get => name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException($"{nameof(name)} can't be empty.");
                }

                name = value;
            }
        }

        public string Description { get; set; }

        public Money price;
        public Money Price
        {
            get => price;
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(price));
                }

                price = value;
            }
        }

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
