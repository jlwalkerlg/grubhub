using System;
using System.Threading.Tasks;
using Shouldly;
using Web.Features.Orders.PlaceOrder;
using Xunit;

namespace WebTests.Features.Orders.PlaceOrder
{
    public class PlaceOrderActionTests : HttpTestBase
    {
        public PlaceOrderActionTests(HttpTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Requires_Authentication()
        {
            var request = new PlaceOrderRequest();

            var response = await fixture.GetClient().Post(
                $"/restaurants/{Guid.NewGuid()}/orders",
                request);

            response.StatusCode.ShouldBe(401);
        }

        [Fact]
        public async Task It_Returns_Validation_Errors()
        {
            var request = new PlaceOrderRequest();

            var response = await fixture.GetAuthenticatedClient().Post(
                $"/restaurants/{Guid.NewGuid()}/orders",
                request);

            response.StatusCode.ShouldBe(422);

            var errors = response.GetErrors();

            errors.ShouldContainKey("mobile");
            errors.ShouldContainKey("addressLine1");
            errors.ShouldContainKey("city");
            errors.ShouldContainKey("postcode");
        }

        [Fact]
        public async Task It_Returns_Handler_Errors()
        {
            var request = new PlaceOrderRequest()
            {
                Mobile = "07123456789",
                AddressLine1 = "12 Maine Road, Manchester, UK",
                City = "Manchester",
                Postcode = "MN12 1NM",
            };

            var response = await fixture.GetAuthenticatedClient().Post(
                $"/restaurants/{Guid.NewGuid()}/orders",
                request);

            response.StatusCode.ShouldBe(400);
            response.GetErrorMessage().ShouldBe(fixture.HandlerErrorMessage);
        }
    }
}
