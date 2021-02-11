using System;
using Web.Services.Authentication;

namespace Web.Features.Orders.PlaceOrder
{
    [Authenticate]
    public record PlaceOrderCommand : IRequest<PlaceOrderResponse>
    {
        public Guid RestaurantId { get; init; }
        public string AddressLine1 { get; init; }
        public string AddressLine2 { get; init; }
        public string AddressLine3 { get; init; }
        public string City { get; init; }
        public string Postcode { get; init; }
    }
}
