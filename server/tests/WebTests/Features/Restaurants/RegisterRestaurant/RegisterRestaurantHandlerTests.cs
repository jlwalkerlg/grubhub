using Shouldly;
using System;
using System.Threading.Tasks;
using Web;
using Web.Domain;
using Web.Domain.Users;
using Web.Features.Restaurants.RegisterRestaurant;
using Web.Services.Geocoding;
using WebTests.Doubles;
using Xunit;

namespace WebTests.Features.Restaurants.RegisterRestaurant
{
    public class RegisterRestaurantHandlerTests
    {
        private readonly UnitOfWorkSpy unitOfWorkSpy;
        private readonly GeocoderSpy geocoderSpy;
        private readonly RegisterRestaurantHandler handler;

        public RegisterRestaurantHandlerTests()
        {
            unitOfWorkSpy = new UnitOfWorkSpy();

            geocoderSpy = new GeocoderSpy();

            handler = new RegisterRestaurantHandler(
                new HasherFake(),
                unitOfWorkSpy,
                geocoderSpy);
        }

        [Fact]
        public async Task It_Returns_A_Validation_Error_If_The_Email_Is_Already_Taken()
        {
            await unitOfWorkSpy.Users.Add(new RestaurantManager(
                new UserId(Guid.NewGuid()),
                "Jordan",
                "Walker",
                new Email("taken@gmail.com"),
                "password123"));

            geocoderSpy.LookupCoordinatesResult = Result.Ok(new Coordinates(54.0f, -2.0f));

            var command = new RegisterRestaurantCommand()
            {
                ManagerFirstName = "Jordan",
                ManagerLastName = "Walker",
                ManagerEmail = "taken@gmail.com",
                ManagerPassword = "password123",
                RestaurantName = "Chow Main",
                RestaurantPhoneNumber = "01234567890",
                AddressLine1 = "1 Maine Road, Manchester, UK",
                AddressLine2 = null,
                City = "Manchester",
                Postcode = "MN12 1NM",
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeAnError();
            result.Error.Type.ShouldBe(ErrorType.ValidationError);
            result.Errors.ShouldContainKey(nameof(command.ManagerEmail));
        }

        [Fact]
        public async Task It_Fails_If_Geocoding_Fails()
        {
            geocoderSpy.LookupCoordinatesResult = Error.Internal("Geocoding failed.");

            var command = new RegisterRestaurantCommand()
            {
                ManagerFirstName = "Jordan",
                ManagerLastName = "Walker",
                ManagerEmail = "test@email.com",
                ManagerPassword = "password123",
                RestaurantName = "Chow Main",
                RestaurantPhoneNumber = "01234567890",
                AddressLine1 = "1 Maine Road, Manchester, UK",
                AddressLine2 = null,
                City = "Manchester",
                Postcode = "MN12 1NM",
            };

            var result = await handler.Handle(command, default);

            result.ShouldBeAnError();
            result.Error.Type.ShouldBe(ErrorType.BadRequest);
        }
    }
}
