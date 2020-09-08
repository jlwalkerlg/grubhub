using System;
using System.Threading.Tasks;
using FoodSnap.Application;
using FoodSnap.Web.Actions.Restaurants.GetAuthUserRestaurant;
using FoodSnap.Web.Envelopes;
using FoodSnap.Web.Queries.Restaurants;
using FoodSnap.WebTests.Doubles;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace FoodSnap.WebTests.Actions.Restaurants.GetAuthUserRestaurant
{
    public class GetAuthUserRestaurantActionTests
    {
        private readonly MediatorSpy mediatorSpy;
        private readonly AuthenticatorSpy authenticatorSpy;
        private readonly GetAuthUserRestaurantAction action;

        public GetAuthUserRestaurantActionTests()
        {
            mediatorSpy = new MediatorSpy();
            authenticatorSpy = new AuthenticatorSpy();

            action = new GetAuthUserRestaurantAction(mediatorSpy, authenticatorSpy);
        }

        [Fact]
        public async Task It_Returns_A_200_With_The_Restaurant()
        {
            var managerId = Guid.NewGuid();

            var restaurant = new RestaurantDto
            {
                Id = Guid.NewGuid(),
                ManagerId = managerId,
                Name = "Chow Main",
                PhoneNumber = "01234567890",
                AddressLine1 = "01 Chinese Street",
                AddressLine2 = "",
                Town = "China Town",
                Postcode = "CH121NM",
                Latitude = 1,
                Longitude = 1,
                Status = "Accepted",
            };

            mediatorSpy.Result = Result.Ok(restaurant);
            authenticatorSpy.UserId = managerId;

            var response = await action.Execute() as ObjectResult;
            var envelope = response.Value as DataEnvelope;

            Assert.Equal(200, response.StatusCode);
            Assert.Same(restaurant, envelope.Data);
        }

        [Fact]
        public async Task It_Returns_A_401_If_Unauthenticated()
        {
            authenticatorSpy.UserId = null;

            var response = await action.Execute() as StatusCodeResult;

            Assert.Equal(401, response.StatusCode);
        }
    }
}
