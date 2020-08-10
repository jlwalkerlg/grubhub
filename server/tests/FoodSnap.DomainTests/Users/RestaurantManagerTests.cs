using System;
using FoodSnap.Domain;
using FoodSnap.Domain.Users;
using Xunit;

namespace FoodSnap.DomainTests.Users
{
    public class RestaurantManagerTests
    {
        [Fact]
        public void Disallows_Empty_Guids()
        {
            var name = "Chow Main";
            var email = new Email("valid@test.com");
            var password = "password123";

            Assert.Throws<ArgumentException>(() =>
            {
                new RestaurantManager(name, email, password, Guid.Empty);
            });
        }
    }
}
