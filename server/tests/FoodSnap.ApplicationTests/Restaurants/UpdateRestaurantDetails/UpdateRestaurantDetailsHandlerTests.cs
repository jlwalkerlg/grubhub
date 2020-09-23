using System;
using System.Threading;
using System.Threading.Tasks;
using FoodSnap.Application.Restaurants.UpdateRestaurantDetails;
using FoodSnap.Domain;
using FoodSnap.Domain.Restaurants;
using Xunit;
using static FoodSnap.Application.Error;

namespace FoodSnap.ApplicationTests.Restaurants.UpdateRestaurantDetails
{
    public class UpdateRestaurantDetailsHandlerTests
    {
        private readonly UnitOfWorkSpy unitOfWorkSpy;
        private readonly UpdateRestaurantDetailsHandler handler;

        public UpdateRestaurantDetailsHandlerTests()
        {
            unitOfWorkSpy = new UnitOfWorkSpy();

            handler = new UpdateRestaurantDetailsHandler(unitOfWorkSpy);
        }

        [Fact]
        public async Task It_Updates_The_Restaurants_Details()
        {
            var restaurant = new Restaurant(
                Guid.NewGuid(),
                "Chow Main",
                new PhoneNumber("01234567890"),
                new Address(
                    "12 Missa Few",
                    "",
                    "NinetyNine",
                    new Postcode("ON33NO")),
                new Coordinates(1, 2));
            await unitOfWorkSpy.Restaurants.Add(restaurant);

            var command = new UpdateRestaurantDetailsCommand
            {
                Id = restaurant.Id,
                Name = "Kung Flu",
                PhoneNumber = "09876543210"
            };

            var result = await handler.Handle(command, default(CancellationToken));

            Assert.True(result.IsSuccess);

            Assert.Equal(command.Name, restaurant.Name);
            Assert.Equal(command.PhoneNumber, restaurant.PhoneNumber.Number);

            Assert.True(unitOfWorkSpy.Commited);
        }

        [Fact]
        public async Task It_Returns_Not_Found_Error_If_Restaurant_Not_Found()
        {
            var command = new UpdateRestaurantDetailsCommand
            {
                Id = Guid.NewGuid(),
                Name = "Kung Flu",
                PhoneNumber = "09876543210"
            };

            var result = await handler.Handle(command, default(CancellationToken));

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorType.NotFound, result.Error.Type);
        }
    }
}
