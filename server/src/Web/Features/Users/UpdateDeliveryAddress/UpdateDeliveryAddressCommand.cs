namespace Web.Features.Users.UpdateDeliveryAddress
{
    public record UpdateDeliveryAddressCommand : IRequest
    {
        public string AddressLine1 { get; init; }
        public string AddressLine2 { get; init; }
        public string City { get; init; }
        public string Postcode { get; init; }
    }
}
