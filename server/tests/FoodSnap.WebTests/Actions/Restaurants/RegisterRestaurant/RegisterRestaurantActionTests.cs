using System.Threading.Tasks;
using FoodSnap.Application;
using FoodSnap.Application.Restaurants.RegisterRestaurant;
using FoodSnap.Web.Actions.Restaurants.RegisterRestaurant;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace FoodSnap.WebTests.Actions.Restaurants.RegisterRestaurant
{
    public class RegisterRestaurantActionTests
    {
        private readonly MediatorSpy mediatorSpy;
        private readonly RegisterRestaurantAction action;

        public RegisterRestaurantActionTests()
        {
            mediatorSpy = new MediatorSpy();

            action = new RegisterRestaurantAction(mediatorSpy);
        }

        [Fact]
        public async Task It_Returns_201_On_Success()
        {
            var command = new RegisterRestaurantCommand
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

            mediatorSpy.Result = Result.Ok();

            var result = await action.Execute(command) as StatusCodeResult;

            Assert.Equal(201, result.StatusCode);
        }
    }
}
