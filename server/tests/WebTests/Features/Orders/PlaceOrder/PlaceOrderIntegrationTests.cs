using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Web;
using Web.Features.Billing;
using Web.Features.Orders.PlaceOrder;
using Web.Services.DateTimeServices;
using WebTests.Doubles;
using WebTests.TestData;
using Xunit;

namespace WebTests.Features.Orders.PlaceOrder
{
    public class PlaceOrderIntegrationTests : IntegrationTestBase
    {
        public PlaceOrderIntegrationTests(IntegrationTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Places_An_Order()
        {
            var now = DateTime.UtcNow;

            var paymentIntent = new PaymentIntent()
            {
                Id = Guid.NewGuid().ToString(),
                ClientSecret = Guid.NewGuid().ToString(),
            };

            using var factory = this.factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddSingleton<IDateTimeProvider>(
                        new DateTimeProviderStub()
                        {
                            UtcNow = now,
                        });

                    services.AddSingleton<IBillingService>(
                        new BillingServiceSpy()
                        {
                            PaymentIntentResult = Result.Ok(paymentIntent),
                        });
                });
            });

            var restaurant = new Restaurant();
            var menu = restaurant.Menu;
            var category = new MenuCategory();
            var menuItem = new MenuItem();
            category.Items.Add(menuItem);
            menu.Categories.Add(category);

            var user = new User();

            var basket = new Basket();
            basket.User = user;
            basket.Restaurant = restaurant;
            var basketItem = new BasketItem();
            basketItem.MenuItem = menuItem;
            basketItem.Quantity = 2;
            basket.Items.Add(basketItem);

            Insert(restaurant, user, basket);

            var request = new PlaceOrderRequest()
            {
                Mobile = "07123456789",
                AddressLine1 = "12 Maine Road",
                AddressLine2 = "Oldham",
                City = "Manchester",
                Postcode = "MN12 1NM",
            };

            var response = await factory.GetAuthenticatedClient(user.Id).Post(
                $"/restaurants/{restaurant.Id}/orders",
                request);

            response.StatusCode.ShouldBe(200);

            var orderId = await response.GetData<string>();

            var order = UseTestDbContext(db => db.Orders.Single());

            order.Id.ShouldBe(orderId);
            order.UserId.ShouldBe(user.Id);
            order.RestaurantId.ShouldBe(restaurant.Id);
            order.DeliveryFee.ShouldBe(restaurant.DeliveryFee);
            order.Status.ShouldBe(Web.Domain.Orders.OrderStatus.Placed);
            order.MobileNumber.ShouldBe(request.Mobile);
            order.Address.ShouldBe("12 Maine Road, Oldham, Manchester, MN12 1NM");
            order.PlacedAt.ShouldBe(now, TimeSpan.FromSeconds(0.000001));
            order.PaymentIntentId.ShouldBe(paymentIntent.Id);
            order.PaymentIntentClientSecret.ShouldBe(paymentIntent.ClientSecret);

            var orderItem = UseTestDbContext(db => db.OrderItems.Single());

            orderItem.MenuItemId.ShouldBe(basketItem.MenuItemId);
            orderItem.Name.ShouldBe(menuItem.Name);
            orderItem.Price.ShouldBe(menuItem.Price);
            orderItem.Quantity.ShouldBe(basketItem.Quantity);
        }
    }
}
