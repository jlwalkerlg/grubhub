using System;

namespace Web.Domain.Restaurants
{
    public class DeliveryLocation
    {
        public DeliveryLocation(Address address, Coordinates coordinates)
        {
            Address = address ?? throw new ArgumentNullException(nameof(address));
            Coordinates = coordinates ?? throw new ArgumentNullException(nameof(coordinates));
        }

        public Address Address { get; }
        public Coordinates Coordinates { get; }
    }
}
