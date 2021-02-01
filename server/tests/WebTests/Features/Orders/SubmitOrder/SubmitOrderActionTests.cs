using System;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace WebTests.Features.Orders.SubmitOrder
{
    public class SubmitOrderActionTests : HttpTestBase
    {
        public SubmitOrderActionTests(HttpTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Requires_Authentication()
        {
            var response = await fixture.GetClient().Put(
                $"/order/{Guid.NewGuid()}/submit");

            response.StatusCode.ShouldBe(401);
        }

        [Fact]
        public async Task It_Returns_Handler_Errors()
        {
            var response = await fixture.GetAuthenticatedClient().Put(
                $"/order/{Guid.NewGuid()}/submit");

            response.StatusCode.ShouldBe(400);
            response.GetErrorMessage().ShouldBe(fixture.HandlerErrorMessage);
        }
    }
}
