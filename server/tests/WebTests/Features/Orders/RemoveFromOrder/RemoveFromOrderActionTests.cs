using System;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace WebTests.Features.Orders.RemoveFromOrder
{
    public class RemoveFromOrderActionTests : HttpTestBase
    {
        public RemoveFromOrderActionTests(HttpTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Requires_Authentication()
        {
            var response = await fixture.GetClient().Delete(
                $"/order/{Guid.NewGuid()}/items/{Guid.NewGuid()}");

            response.StatusCode.ShouldBe(401);
        }

        [Fact]
        public async Task It_Returns_Handler_Errors()
        {
            var response = await fixture.GetAuthenticatedClient().Delete(
                $"/order/{Guid.NewGuid()}/items/{Guid.NewGuid()}");

            response.StatusCode.ShouldBe(400);
            response.GetErrorMessage().ShouldBe(fixture.HandlerErrorMessage);
        }
    }
}
