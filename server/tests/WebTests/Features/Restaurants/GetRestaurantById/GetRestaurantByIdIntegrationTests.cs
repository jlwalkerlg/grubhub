using System;
using System.Threading.Tasks;
using Shouldly;
using Web.Features.Restaurants;
using WebTests.TestData;
using Xunit;

namespace WebTests.Features.Restaurants.GetRestaurantById
{
    public class GetRestaurantByIdIntegrationTests : IntegrationTestBase
    {
        public GetRestaurantByIdIntegrationTests(IntegrationTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Returns_The_Restaurant()
        {
            var manager = new User();

            var restaurant = new Restaurant()
            {
                ManagerId = manager.Id,
            };

            fixture.Insert(manager, restaurant);

            var response = await fixture.GetClient().Get($"/restaurants/{restaurant.Id}");

            response.StatusCode.ShouldBe(200);

            var restaurantDto = await response.GetData<RestaurantDto>();

            restaurantDto.Id.ShouldBe(restaurant.Id);
            restaurantDto.ManagerId.ShouldBe(manager.Id);
            restaurantDto.Name.ShouldBe(restaurant.Name);
            restaurantDto.PhoneNumber.ShouldBe(restaurant.PhoneNumber);
            restaurantDto.Status.ShouldBe(restaurant.Status);
            restaurantDto.Address.ShouldBe(restaurant.Address);
            restaurantDto.Latitude.ShouldBe(restaurant.Latitude);
            restaurantDto.Longitude.ShouldBe(restaurant.Longitude);
            restaurantDto.OpeningTimes.Monday.ShouldBeNull();
            restaurantDto.OpeningTimes.Tuesday.ShouldBeNull();
            restaurantDto.OpeningTimes.Wednesday.ShouldBeNull();
            restaurantDto.OpeningTimes.Thursday.ShouldBeNull();
            restaurantDto.OpeningTimes.Friday.ShouldBeNull();
            restaurantDto.OpeningTimes.Saturday.ShouldBeNull();
            restaurantDto.OpeningTimes.Sunday.ShouldBeNull();
            restaurantDto.DeliveryFee.ShouldBe(restaurant.DeliveryFee);
            restaurantDto.MaxDeliveryDistanceInKm.ShouldBe(restaurant.MaxDeliveryDistanceInKm);
            restaurantDto.MinimumDeliverySpend.ShouldBe(restaurant.MinimumDeliverySpend);
            restaurantDto.EstimatedDeliveryTimeInMinutes.ShouldBe(restaurant.EstimatedDeliveryTimeInMinutes);
        }

        [Fact]
        public async Task It_Fails_If_The_Restaurant_Is_Not_Found()
        {
            var response = await fixture.GetClient().Get($"/restaurants/{Guid.NewGuid()}");

            response.StatusCode.ShouldBe(404);
        }
    }
}
