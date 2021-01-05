using System;
using System.Collections.Generic;

namespace Web.Domain.Restaurants
{
    public class Cuisine : Entity<Cuisine>
    {
        public Cuisine(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name must not be empty.");
            }

            Name = name;
        }

        private Cuisine() { }

        public string Name { get; }

        // ef core
        private List<Restaurant> Restaurants { get; } = new();

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        protected override bool IdentityEquals(Cuisine other)
        {
            return Name == other.Name;
        }
    }
}
