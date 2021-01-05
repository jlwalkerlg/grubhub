using System;

namespace Web.Features.Users
{
    public record UserDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string Email { get; init; }
        public string Role { get; init; }
        public Guid? RestaurantId { get; init; }
        public string RestaurantName { get; init; }
    }
}
