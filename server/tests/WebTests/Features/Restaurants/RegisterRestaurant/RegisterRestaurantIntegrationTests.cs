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

            var fixture = this.fixture.WithServices(services =>
            {
                services.AddSingleton<IClock>(
                    new ClockStub()
                    {
                        UtcNow = now,
                    }
                );
            });

            var request = new RegisterRestaurantCommand()
            {
                ManagerName = "Jordan Walker",
                ManagerEmail = "walker.jlg@gmail.com",
                ManagerPassword = "password123",
                RestaurantName = "Chow Main",
                RestaurantPhoneNumber = "01234567890",
                Address = "1 Maine Road, Manchester, UK",
            };

            var response = await fixture.GetClient().Post("/restaurants/register", request);

            response.StatusCode.ShouldBe(201);

            var restaurant = fixture.UseTestDbContext(db => db.Restaurants.Single());

            restaurant.Name.ShouldBe(request.RestaurantName);
            restaurant.PhoneNumber.ShouldBe(request.RestaurantPhoneNumber);
            restaurant.Address.ShouldBe(GeocoderStub.Address);
            restaurant.Latitude.ShouldBe(GeocoderStub.Latitude);
            restaurant.Longitude.ShouldBe(GeocoderStub.Longitude);
            restaurant.Status.ShouldBe(Web.Domain.Restaurants.RestaurantStatus.PendingApproval);
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
            manager.Role.ShouldBe(Web.Domain.Users.UserRole.RestaurantManager);

            fixture.VerifyHash(request.ManagerPassword, manager.Password).ShouldBe(true);

            var ev = fixture.UseTestDbContext(db => db.Events.Single());

            var rEv = ev.ToEvent() as RestaurantRegisteredEvent;

            rEv.CreatedAt.ShouldBe(now, TimeSpan.FromSeconds(0.000001));
            rEv.ManagerId.Value.ShouldBe(manager.Id);
            rEv.RestaurantId.Value.ShouldBe(restaurant.Id);
        }
    }
}
