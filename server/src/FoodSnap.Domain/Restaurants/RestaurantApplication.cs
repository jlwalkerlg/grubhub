using System;

namespace FoodSnap.Domain.Restaurants
{
    public class RestaurantApplication : Entity
    {
        public Guid RestaurantId { get; }
        public RestaurantApplicationStatus Status { get; private set; }
        public bool Accepted => Status == RestaurantApplicationStatus.Accepted;

        public RestaurantApplication(Guid restaurantId)
        {
            if (restaurantId == Guid.Empty)
            {
                throw new ArgumentException($"{nameof(restaurantId)} must not be empty.");
            }

            RestaurantId = restaurantId;
            Status = RestaurantApplicationStatus.Pending;
        }

        public void Accept()
        {
            if (Accepted)
            {
                throw new InvalidOperationException("Application already accepted.");
            }

            Status = RestaurantApplicationStatus.Accepted;
        }
    }
}
