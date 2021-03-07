namespace Web.Features.Restaurants.RegisterRestaurant
{
    public record RegisterRestaurantCommand : IRequest
    {
        public string ManagerFirstName { get; init; }
        public string ManagerLastName { get; init; }
        public string ManagerEmail { get; init; }
        public string ManagerPassword { get; init; }
        public string RestaurantName { get; init; }
        public string RestaurantPhoneNumber { get; init; }
        public string AddressLine1 { get; init; }
        public string AddressLine2 { get; init; }
        public string City { get; init; }
        public string Postcode { get; init; }
    }
}
