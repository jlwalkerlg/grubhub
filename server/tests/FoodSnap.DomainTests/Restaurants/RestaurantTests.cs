using System;
using FoodSnap.Domain;
using FoodSnap.Domain.Restaurants;
using Xunit;

namespace FoodSnap.DomainTests.Restaurants
{
    public class RestaurantTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Disallows_Invalid_Names(string name)
        {
            var phoneNumber = new PhoneNumber("01234567890");
            var address = new Address("12 Manchester Road", "", "Manchester", new Postcode("WS12 1WS"));

            Assert.Throws<ArgumentException>(() => new Restaurant(name, phoneNumber, address));
        }

        [Fact]
        public void Disallows_Null_Phone_Numbers()
        {
            var name = "Chow Main";
            var address = new Address("12 Manchester Road", "", "Manchester", new Postcode("WS12 1WS"));

            Assert.Throws<ArgumentNullException>(() => new Restaurant(name, null, address));
        }

        [Fact]
        public void Disallows_Null_Postcodes()
        {
            var name = "Chow Main";
            var phoneNumber = new PhoneNumber("01234567890");

            Assert.Throws<ArgumentNullException>(() => new Restaurant(name, phoneNumber, null));
        }
    }
}
