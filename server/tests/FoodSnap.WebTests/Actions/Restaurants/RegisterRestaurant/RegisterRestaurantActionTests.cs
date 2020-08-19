using System.Threading.Tasks;
using FoodSnap.Application;
using FoodSnap.Application.Restaurants.RegisterRestaurant;
using FoodSnap.Web.Actions.Restaurants.RegisterRestaurant;
using Xunit;

namespace FoodSnap.WebTests.Actions.Restaurants.RegisterRestaurant
{
    public class RegisterRestaurantActionTests
    {
        private readonly MediatorSpy mediatorSpy;
        private readonly PresenterSpy<RegisterRestaurantCommand, Result> presenterSpy;
        private readonly RegisterRestaurantAction action;
        private readonly RegisterRestaurantRequest request;

        public RegisterRestaurantActionTests()
        {
            mediatorSpy = new MediatorSpy();
            presenterSpy = new PresenterSpy<RegisterRestaurantCommand, Result>();

            action = new RegisterRestaurantAction(mediatorSpy, presenterSpy);

            request = new RegisterRestaurantRequest
            {
                ManagerName = "Jordan Walker",
                ManagerEmail = "test@email.com",
                ManagerPassword = "password123",
                RestaurantName = "Chow Main",
                RestaurantPhoneNumber = "01234567890",
                AddressLine1 = "12 Manchester Road",
                AddressLine2 = "",
                Town = "Manchester",
                Postcode = "MN12 1NM"
            };
        }

        [Fact]
        public async Task It_Sends_The_Command_To_Mediator()
        {
            await action.Execute(request);

            var command = mediatorSpy.Request as RegisterRestaurantCommand;

            Assert.NotNull(command);
            Assert.Equal(request.ManagerName, command.ManagerName);
            Assert.Equal(request.ManagerEmail, command.ManagerEmail);
            Assert.Equal(request.ManagerPassword, command.ManagerPassword);
            Assert.Equal(request.RestaurantName, command.RestaurantName);
            Assert.Equal(request.RestaurantPhoneNumber, command.RestaurantPhoneNumber);
            Assert.Equal(request.AddressLine1, command.AddressLine1);
            Assert.Equal(request.AddressLine2, command.AddressLine2);
            Assert.Equal(request.Town, command.Town);
            Assert.Equal(request.Postcode, command.Postcode);
        }

        [Fact]
        public async Task It_Returns_The_Presenter_Result()
        {
            var result = await action.Execute(request);

            Assert.Same(presenterSpy.Result, result);
        }
    }
}
