using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using Web.Features.Restaurants.RegisterRestaurant;
using Web.Services.DateTimeServices;
using Web.Services.Hashing;
using WebTests.Doubles;
using Xunit;

namespace WebTests.Features.Restaurants.RegisterRestaurant
{
    public class RegisterRestaurantIntegrationTests : IntegrationTestBase
    {
        private readonly IHasher hasher;

        public RegisterRestaurantIntegrationTests(IntegrationTestFixture fixture) : base(fixture)
        {
            hasher = factory.Services.GetRequiredService<IHasher>();
        }

        [Fact]
        public async Task It_Registers_A_Restaurant_And_A_Manager()
        {
            var now = DateTimeOffset.UtcNow;

            using var factory = this.factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddSingleton<IDateTimeProvider>(
                        new DateTimeProviderStub()
                        {
                            UtcNow = now,
                        }
                    );
                });
            });

            var request = new RegisterRestaurantCommand()
            {
                ManagerFirstName = "Jordan",
                ManagerLastName = "Walker",
                ManagerEmail = "walker.jlg@gmail.com",
                ManagerPassword = "password123",
                RestaurantName = "Chow Main",
                RestaurantPhoneNumber = "01234567890",
                AddressLine1 = "1 Maine Road, Manchester, UK",
                AddressLine2 = null,
                City = "Manchester",
                Postcode = "MN12 1NM",
            };

            var response = await factory.GetClient().Post("/restaurants/register", request);

            response.StatusCode.ShouldBe(201);

            var restaurant = UseTestDbContext(db => db.Restaurants.Single());

            restaurant.Name.ShouldBe(request.RestaurantName);
            restaurant.PhoneNumber.ShouldBe(request.RestaurantPhoneNumber);
            restaurant.AddressLine1.ShouldBe(request.AddressLine1);
            restaurant.AddressLine2.ShouldBe(request.AddressLine2);
            restaurant.City.ShouldBe(request.City);
            restaurant.Postcode.ShouldBe(request.Postcode);
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

            var manager = UseTestDbContext(db => db.Users.Single());

            manager.Id.ShouldBe(restaurant.ManagerId);
            manager.Email.ShouldBe(request.ManagerEmail);
            manager.FirstName.ShouldBe(request.ManagerFirstName);
            manager.LastName.ShouldBe(request.ManagerLastName);
            manager.Role.ShouldBe(Web.Domain.Users.UserRole.RestaurantManager);

            hasher.CheckMatch(request.ManagerPassword, manager.Password).ShouldBe(true);
        }
    }
}
