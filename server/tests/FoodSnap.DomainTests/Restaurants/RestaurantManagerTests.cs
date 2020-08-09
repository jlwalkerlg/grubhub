using System;
using FoodSnap.Domain;
using FoodSnap.Domain.Restaurants;
using Xunit;

namespace FoodSnap.DomainTests.Restaurants
{
    public class RestaurantManagerTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Disallows_Invalid_Names(string name)
        {
            var email = new Email("valid@test.com");
            var password = "password123";

            Assert.Throws<ArgumentException>(() =>
            {
                new RestaurantManager(name, email, password, Guid.NewGuid());
            });
        }

        [Fact]
        public void Disallows_Null_Emails()
        {
            var name = "Chow Main";
            var password = "password123";

            Assert.Throws<ArgumentNullException>(() =>
            {
                new RestaurantManager(name, null, password, Guid.NewGuid());
            });
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Disallows_Invalid_Passwords(string password)
        {
            var name = "Mr Wong";
            var email = new Email("valid@test.com");

            Assert.Throws<ArgumentException>(() =>
            {
                new RestaurantManager(name, email, password, Guid.NewGuid());
            });
        }

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
