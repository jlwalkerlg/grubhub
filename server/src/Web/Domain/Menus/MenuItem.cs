using System;

namespace Web.Domain.Menus
{
    public class MenuItem : Entity<MenuItem>
    {
        private string name;
        private string description;
        private Money price;

        internal MenuItem(Guid id, string name, string description, Money price)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Id must not be empty.");
            }

            Id = id;
            Name = name;
            Description = description;
            Price = price;
        }

        public Guid Id { get; }

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
