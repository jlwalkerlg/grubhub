using System;

namespace Web.Features.Orders.PlaceOrder
{
    public record PlaceOrderCommand : IRequest<string>
    {
        public Guid RestaurantId { get; init; }
        public string Mobile { get; init; }
        public string AddressLine1 { get; init; }
        public string AddressLine2 { get; init; }
        public string City { get; init; }
        public string Postcode { get; init; }
    }
}
