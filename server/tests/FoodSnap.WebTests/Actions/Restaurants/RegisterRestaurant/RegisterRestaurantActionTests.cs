using System.Threading.Tasks;
using FoodSnap.Application.Restaurants.RegisterRestaurant;
using FoodSnap.Shared;
using FoodSnap.Web.Actions.Restaurants.RegisterRestaurant;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace FoodSnap.WebTests.Actions.Restaurants.RegisterRestaurant
{
    public class RegisterRestaurantActionTests
    {
        private readonly SenderSpy senderSpy;
        private readonly RegisterRestaurantAction action;

        public RegisterRestaurantActionTests()
        {
            senderSpy = new SenderSpy();

            action = new RegisterRestaurantAction(senderSpy);
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
                Address = "1 Maine Road, Manchester, UK"
            };

            senderSpy.Result = Result.Ok();

            var result = await action.Execute(command) as StatusCodeResult;

            Assert.Equal(201, result.StatusCode);
        }
    }
}
