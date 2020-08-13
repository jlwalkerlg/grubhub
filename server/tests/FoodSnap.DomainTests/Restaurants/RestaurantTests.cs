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
            var coordinates = new Coordinates(0, 0);

            Assert.Throws<ArgumentException>(() => new Restaurant(name, phoneNumber, address, coordinates));
        }

        [Fact]
        public void Disallows_Null_Phone_Numbers()
        {
            var name = "Chow Main";
            var address = new Address("12 Manchester Road", "", "Manchester", new Postcode("WS12 1WS"));
            var coordinates = new Coordinates(0, 0);

            Assert.Throws<ArgumentNullException>(() => new Restaurant(name, null, address, coordinates));
        }

        [Fact]
        public void Disallows_Null_Addresses()
        {
            var name = "Chow Main";
            var phoneNumber = new PhoneNumber("01234567890");
            var coordinates = new Coordinates(0, 0);

            Assert.Throws<ArgumentNullException>(() => new Restaurant(name, phoneNumber, null, coordinates));
        }

        [Fact]
        public void Disallows_Null_Coordinates()
        {
            var name = "Chow Main";
            var address = new Address("12 Manchester Road", "", "Manchester", new Postcode("WS12 1WS"));
            var phoneNumber = new PhoneNumber("01234567890");

            Assert.Throws<ArgumentNullException>(() => new Restaurant(name, phoneNumber, address, null));
        }
    }
}
