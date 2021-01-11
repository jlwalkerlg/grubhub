using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using Web.Features.Restaurants.RegisterRestaurant;
using Web.Services;
using WebTests.Doubles;
using Xunit;

namespace WebTests.Features.Restaurants.RegisterRestaurant
{
    public class RegisterRestaurantIntegrationTests : IntegrationTestBase
    {
        public RegisterRestaurantIntegrationTests(IntegrationTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Registers_A_Restaurant_And_A_Manager()
        {
            var now = DateTime.UtcNow;

            using var factory = fixture.CreateFactory(services =>
            {
                services.AddSingleton<IClock>(new ClockStub()
                {
                    UtcNow = now,
                });
            });

            var client = new HttpTestClient(factory);

            var request = new RegisterRestaurantCommand()
            {
                ManagerName = "Jordan Walker",
                ManagerEmail = "walker.jlg@gmail.com",
                ManagerPassword = "password123",
                RestaurantName = "Chow Main",
                RestaurantPhoneNumber = "01234567890",
                Address = "1 Maine Road, Manchester, UK",
            };

            var response = await client.Post("/restaurants/register", request);

            response.StatusCode.ShouldBe(201);

            var restaurant = fixture.UseTestDbContext(db => db.Restaurants.Single());

            restaurant.Name.ShouldBe(request.RestaurantName);
            restaurant.PhoneNumber.ShouldBe(request.RestaurantPhoneNumber);
            restaurant.Address.ShouldBe(GeocoderStub.Address);
            restaurant.Latitude.ShouldBe(GeocoderStub.Latitude);
            restaurant.Longitude.ShouldBe(GeocoderStub.Longitude);
            restaurant.Status.ShouldBe("PendingApproval");
            restaurant.MondayOpen.ShouldBeNull();
            restaurant.MondayClose.ShouldBeNull();
            restaurant.TuesdayOpen.ShouldBeNull();
            restaurant.TuesdayClose.ShouldBeNull();
            restaurant.WednesdayOpen.ShouldBeNull();
            restaurant.WednesdayClose.ShouldBeNull();
            restaurant.ThursdayOpen.ShouldBeNull();
            restaurant.ThursdayClose.ShouldBeNull();
            restaurant.FridayOpen.ShouldBeNull();
            restaurant.FridayClose.ShouldBeNull();
            restaurant.SaturdayOpen.ShouldBeNull();
            restaurant.SaturdayClose.ShouldBeNull();
            restaurant.SundayOpen.ShouldBeNull();
            restaurant.SundayClose.ShouldBeNull();
            restaurant.MinimumDeliverySpend.ShouldBe(0);
            restaurant.DeliveryFee.ShouldBe(0);
            restaurant.MaxDeliveryDistanceInKm.ShouldBe(0);
            restaurant.EstimatedDeliveryTimeInMinutes.ShouldBe(30);

            var manager = fixture.UseTestDbContext(db => db.Users.Single());

            manager.Id.ShouldBe(restaurant.ManagerId);
            manager.Email.ShouldBe(request.ManagerEmail);
            manager.Name.ShouldBe(request.ManagerName);
            manager.Role.ShouldBe("RestaurantManager");

            fixture.VerifyHash(request.ManagerPassword, manager.Password).ShouldBe(true);

            var menu = fixture.UseTestDbContext(db => db.Menus.Single());

            menu.RestaurantId.ShouldBe(restaurant.Id);

            var categories = fixture.UseTestDbContext(db => db.MenuCategories.ToList());

            categories.ShouldBeEmpty();

            var @event = fixture.UseTestDbContext(db => db.Events.Single());

            @event.Type.ShouldBe(typeof(RestaurantRegisteredEvent).ToString());
            @event.CreatedAt.ShouldBe(now, TimeSpan.FromSeconds(0.000001));

            var rEvent = @event.ToEvent<RestaurantRegisteredEvent>();

            rEvent.CreatedAt.ShouldBe(now, TimeSpan.FromSeconds(0.000001));
            rEvent.ManagerId.Value.ShouldBe(manager.Id);
            rEvent.RestaurantId.Value.ShouldBe(restaurant.Id);
        }
    }
}
