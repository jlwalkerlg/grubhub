using System;

namespace Web.Domain.Menus
{
    public class MenuItem : Entity<MenuItem>
    {
        internal MenuItem(string name, string description, Money price)
        {
            Name = name;
            Description = description;
            Price = price;
        }

        private string name;
        public string Name
        {
            get => name;
            internal set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Name must not be empty.");
                }

                name = value;
            }
        }

        private string description;
        public string Description
        {
            get => description;
            set
            {
                if (value?.Length > 280)
                {
                    throw new ArgumentException("Description must not be longer than 280 characters.");
                }

                description = value;
            }
        }

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
