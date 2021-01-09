using System;
using System.Threading.Tasks;
using Shouldly;
using Web.Domain;
using Web.Domain.Restaurants;
using Web.Domain.Users;
using Web.Features.Restaurants.UpdateOpeningHours;
using WebTests.Doubles;
using Xunit;
using static Web.Error;

namespace WebTests.Features.Restaurants.UpdateOpeningTimes
{
    public class UpdateOpeningHoursHandlerTests
    {
        private readonly UnitOfWorkSpy unitOfWorkSpy;
        private readonly AuthenticatorSpy authenticatorSpy;
        private readonly UpdateOpeningHoursHandler handler;

        public UpdateOpeningHoursHandlerTests()
        {
            unitOfWorkSpy = new UnitOfWorkSpy();

            authenticatorSpy = new AuthenticatorSpy();

            handler = new UpdateOpeningHoursHandler(unitOfWorkSpy, authenticatorSpy);
        }

        [Fact]
        public async Task It_Fails_If_The_Restaurant_Is_Not_Found()
        {
            var command = new UpdateOpeningHoursCommand()
            {
                RestaurantId = Guid.NewGuid(),
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeAnError();
            result.Error.Type.ShouldBe(ErrorType.NotFound);
        }

        [Fact]
        public async Task If_Fails_If_The_User_Is_Unauthorised()
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
                new Address("1 Maine Road, Madchester, MN12 1NM"),
                new Coordinates(1, 2));

            await unitOfWorkSpy.UserRepositorySpy.Add(manager);
            await unitOfWorkSpy.RestaurantRepositorySpy.Add(restaurant);

            authenticatorSpy.SignIn(Guid.NewGuid());

            var command = new UpdateOpeningHoursCommand()
            {
                RestaurantId = restaurant.Id,
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeAnError();
            result.Error.Type.ShouldBe(ErrorType.Unauthorised);
        }
    }
}
