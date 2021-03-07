using System;

namespace Web.Features.Users
{
    public record UserDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string Email { get; init; }
        public string Role { get; init; }
        public string MobileNumber { get; init; }
        public string AddressLine1 { get; init; }
        public string AddressLine2 { get; init; }
        public string City { get; init; }
        public string Postcode { get; init; }
        public Guid? RestaurantId { get; init; }
        public string RestaurantName { get; init; }
    }
}
