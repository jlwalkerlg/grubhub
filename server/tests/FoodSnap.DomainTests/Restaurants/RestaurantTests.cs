using System;
using FoodSnap.Domain;
using FoodSnap.Domain.Restaurants;
using FoodSnap.Domain.Users;
using Xunit;

namespace FoodSnap.DomainTests.Restaurants
{
    public class RestaurantTests
    {
        [Fact]
        public void New_Restaurants_Have_Pending_Status()
        {
            var restaurant = new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                new UserId(Guid.NewGuid()),
                "Chow Main",
                new PhoneNumber("01234567890"),
                new Address("1 Maine Road, Manchester, UK"),
                new Coordinates(1, 1));

            Assert.Equal(RestaurantStatus.PendingApproval, restaurant.Status);
        }

        [Fact]
        public void Status_Changes_To_Approved_When_Approved()
        {
            var restaurant = new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                new UserId(Guid.NewGuid()),
                "Chow Main",
                new PhoneNumber("01234567890"),
                new Address("1 Maine Road, Manchester, UK"),
                new Coordinates(1, 1));

            restaurant.Approve();

            Assert.Equal(RestaurantStatus.Approved, restaurant.Status);
        }

        [Fact]
        public void Can_Only_Be_Approved_If_Status_Is_Pending_Approval()
        {
            var restaurant = new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                new UserId(Guid.NewGuid()),
                "Chow Main",
                new PhoneNumber("01234567890"),
                new Address("1 Maine Road, Manchester, UK"),
                new Coordinates(1, 1));

            restaurant.Approve();

            Assert.Throws<InvalidOperationException>(() => restaurant.Approve());
        }

        [Fact]
        public void Two_Restaurants_Are_Equal_When_Their_Ids_Are_The_Same()
        {
            var id = new RestaurantId(Guid.NewGuid());

            var first = new Restaurant(
                id,
                new UserId(Guid.NewGuid()),
                "Chow Main",
                new PhoneNumber("01234567890"),
                new Address("1 Maine Road, Manchester, UK"),
                new Coordinates(0, 0));

            var second = new Restaurant(
                id,
                new UserId(Guid.NewGuid()),
                "Chow Main",
                new PhoneNumber("01234567890"),
                new Address("1 Maine Road, Manchester, UK"),
                new Coordinates(0, 0));

            Assert.Equal(first, second);
            Assert.True(first == second);
            Assert.Equal(first.GetHashCode(), second.GetHashCode());
        }

        [Fact]
        public void Id_Cant_Be_Null()
        {
            var managerId = new UserId(Guid.NewGuid());
            var name = "Chow Main";
            var phoneNumber = new PhoneNumber("01234567890");
            var address = new Address("1 Maine Road, Manchester, UK");
            var coordinates = new Coordinates(0, 0);

            Assert.Throws<ArgumentNullException>(() =>
            {
                new Restaurant(
                    null,
                    managerId,
                    name,
                    phoneNumber,
                    address,
                    coordinates);
            });
        }

        [Fact]
        public void Manager_Id_Cant_Be_Null()
        {
            var name = "Chow Main";
            var phoneNumber = new PhoneNumber("01234567890");
            var address = new Address("1 Maine Road, Manchester, UK");
            var coordinates = new Coordinates(0, 0);

            Assert.Throws<ArgumentNullException>(() =>
            {
                new Restaurant(
                    new RestaurantId(Guid.NewGuid()),
                    null,
                    name,
                    phoneNumber,
                    address,
                    coordinates);
            });
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Disallows_Invalid_Names(string name)
        {
            var phoneNumber = new PhoneNumber("01234567890");
            var address = new Address("1 Maine Road, Manchester, UK");
            var coordinates = new Coordinates(0, 0);

            Assert.Throws<ArgumentException>(() =>
            {
                new Restaurant(
                    new RestaurantId(Guid.NewGuid()),
                    new UserId(Guid.NewGuid()),
                    name,
                    phoneNumber,
                    address,
                    coordinates);
            });

            var restaurant = new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                new UserId(Guid.NewGuid()),
                "Jordan Walker",
                phoneNumber,
                address,
                coordinates);

            Assert.Throws<ArgumentException>(() =>
            {
                restaurant.Name = name;
            });

        }

        [Fact]
        public void Disallows_Null_Phone_Numbers()
        {
            var name = "Chow Main";
            var address = new Address("1 Maine Road, Manchester, UK");
            var coordinates = new Coordinates(0, 0);

            Assert.Throws<ArgumentNullException>(() =>
            {
                new Restaurant(
                    new RestaurantId(Guid.NewGuid()),
                    new UserId(Guid.NewGuid()),
                    name,
                    null,
                    address,
                    coordinates);
            });

            var restaurant = new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                new UserId(Guid.NewGuid()),
                name,
                new PhoneNumber("01234567890"),
                address,
                coordinates);

            Assert.Throws<ArgumentNullException>(() =>
            {
                restaurant.PhoneNumber = null;
            });
        }

        [Fact]
        public void Disallows_Null_Addresses()
        {
            var name = "Chow Main";
            var phoneNumber = new PhoneNumber("01234567890");
            var coordinates = new Coordinates(0, 0);

            Assert.Throws<ArgumentNullException>(() =>
            {
                new Restaurant(
                    new RestaurantId(Guid.NewGuid()),
                    new UserId(Guid.NewGuid()),
                    name,
                    phoneNumber,
                    null,
                    coordinates);
            });
        }

        [Fact]
        public void Disallows_Null_Coordinates()
        {
            var name = "Chow Main";
            var address = new Address("1 Maine Road, Manchester, UK");
            var phoneNumber = new PhoneNumber("01234567890");

            Assert.Throws<ArgumentNullException>(() =>
            {
                new Restaurant(
                    new RestaurantId(Guid.NewGuid()),
                    new UserId(Guid.NewGuid()),
                    name,
                    phoneNumber,
                    address,
                    null);
            });
        }
    }
}
