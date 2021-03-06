using Shouldly;
using System;
using System.Threading.Tasks;
using Web;
using Web.Domain;
using Web.Domain.Restaurants;
using Web.Domain.Users;
using Web.Features.Restaurants.UpdateRestaurantDetails;
using WebTests.Doubles;
using Xunit;

namespace WebTests.Features.Restaurants.UpdateRestaurantDetails
{
    public class UpdateRestaurantDetailsHandlerTests
    {
        private readonly UnitOfWorkSpy unitOfWorkSpy;
        private readonly AuthenticatorSpy authenticatorSpy;
        private readonly UpdateRestaurantDetailsHandler handler;

        public UpdateRestaurantDetailsHandlerTests()
        {
            unitOfWorkSpy = new UnitOfWorkSpy();
            authenticatorSpy = new AuthenticatorSpy();

            handler = new UpdateRestaurantDetailsHandler(unitOfWorkSpy, authenticatorSpy);
        }

        [Fact]
        public async Task It_Fails_If_The_Restaurant_Is_Not_Found()
        {
            var command = new UpdateRestaurantDetailsCommand()
            {
                RestaurantId = Guid.NewGuid(),
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeAnError();
            result.Error.Type.ShouldBe(ErrorType.NotFound);
        }

        [Fact]
        public async Task It_Fails_If_The_User_Is_Unauthorised()
        {
            var manager = new RestaurantManager(
                new UserId(Guid.NewGuid()),
                "Jordan Walker",
                new Email("walker.jlg@gmail.com"),
                "password123");

            var restaurant = new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                manager.Id,
                "Chow Main",
                new PhoneNumber("01234567890"),
                new Address(
                    "12 Maine Road",
                    null,
                    "Manchester",
                    new Postcode("MN12 1NM")),
                new Coordinates(1, 2));

            await unitOfWorkSpy.Users.Add(manager);
            await unitOfWorkSpy.Restaurants.Add(restaurant);

            await authenticatorSpy.SignIn(Guid.NewGuid());

            var command = new UpdateRestaurantDetailsCommand()
            {
                RestaurantId = restaurant.Id,
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeAnError();
            result.Error.Type.ShouldBe(ErrorType.Unauthorised);
        }
    }
}
