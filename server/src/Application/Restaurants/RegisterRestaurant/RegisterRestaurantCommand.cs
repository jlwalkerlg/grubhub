namespace Application.Restaurants.RegisterRestaurant
{
    public record RegisterRestaurantCommand : IRequest
    {
        public string ManagerName { get; init; }
        public string ManagerEmail { get; init; }
        public string ManagerPassword { get; init; }
        public string RestaurantName { get; init; }
        public string RestaurantPhoneNumber { get; init; }
        public string Address { get; init; }
    }
}
