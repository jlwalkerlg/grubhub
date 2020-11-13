using System;

namespace FoodSnap.Application.Users
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public Guid? RestaurantId { get; set; }
        public string RestaurantName { get; set; }
    }
}