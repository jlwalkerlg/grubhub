using System;

namespace FoodSnap.Domain.Users
{
    public class RestaurantManager : User
    {
        public Guid RestaurantId { get; }

        protected override UserRole Role => UserRole.RestaurantManager;

        public RestaurantManager(
            string name,
            Email email,
            string password,
            Guid restaurantId) : base(name, email, password)
        {
            if (restaurantId == Guid.Empty)
            {
                throw new ArgumentException($"{nameof(restaurantId)} must not be empty.");
            }

            RestaurantId = restaurantId;
        }

        // EF Core
        private RestaurantManager() { }
    }
}
