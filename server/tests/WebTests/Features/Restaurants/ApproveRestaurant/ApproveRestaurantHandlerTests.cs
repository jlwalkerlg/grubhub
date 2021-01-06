using System;
using System.Linq;
using System.Threading.Tasks;
using Web.Domain;
using Web.Domain.Restaurants;
using Web.Domain.Users;
using Web.Features.Restaurants;
using Web.Features.Restaurants.ApproveRestaurant;
using WebTests.Doubles;
using Xunit;

namespace WebTests.Features.Restaurants.ApproveRestaurant
{
    public class ApproveRestaurantHandlerTests
    {
        private readonly UnitOfWorkSpy unitOfWorkSpy;
        private readonly ApproveRestaurantHandler handler;

        public ApproveRestaurantHandlerTests()
        {
            unitOfWorkSpy = new UnitOfWorkSpy();

            handler = new ApproveRestaurantHandler(unitOfWorkSpy, new ClockStub());
        }

        [Fact]
        public async Task It_Approves_The_Restaurant()
        {
            var restaurant = new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                new UserId(Guid.NewGuid()),
                "Chow Main",
                new PhoneNumber("01234567890"),
                new Address("1 Maine Road, Manchester, UK"),
                new Coordinates(0, 0));

            await unitOfWorkSpy.Restaurants.Add(restaurant);

            var command = new ApproveRestaurantCommand
            {
                RestaurantId = restaurant.Id.Value,
            };
            var result = await handler.Handle(command, default);

            Assert.True(result.IsSuccess);
            Assert.Equal(RestaurantStatus.Approved, restaurant.Status);
            Assert.True(unitOfWorkSpy.Commited);
        }

        [Fact]
        public async Task It_Creates_A_Restaurant_Approved_Event()
        {
            var restaurant = new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                new UserId(Guid.NewGuid()),
                "Chow Main",
                new PhoneNumber("01234567890"),
                new Address("1 Maine Road, Manchester, UK"),
                new Coordinates(0, 0));

            await unitOfWorkSpy.Restaurants.Add(restaurant);

            var command = new ApproveRestaurantCommand
            {
                RestaurantId = restaurant.Id.Value,
            };
            var result = await handler.Handle(command, default);

            var @event = (await unitOfWorkSpy.EventRepositorySpy.All())
                .OfType<RestaurantApprovedEvent>()
                .Single();

            Assert.Equal(@event.RestaurantId, restaurant.Id);
        }

        [Fact]
        public async Task It_Returns_An_Error_If_The_Restaurant_Was_Not_Found()
        {
            var command = new ApproveRestaurantCommand
            {
                RestaurantId = Guid.NewGuid()
            };
            var result = await handler.Handle(command, default);

            Assert.False(result.IsSuccess);
        }
    }
}
