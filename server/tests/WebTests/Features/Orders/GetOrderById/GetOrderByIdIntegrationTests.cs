using System;
using System.Threading.Tasks;
using WebTests.TestData;
using Xunit;

namespace WebTests.Features.Orders.GetOrderById
{
    public class GetOrderByIdIntegrationTests : IntegrationTestBase
    {
        public GetOrderByIdIntegrationTests(IntegrationTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task It_Fails_If_The_User_Is_Unauthorised()
        {
            var order = new Order();

            Insert(order);

            var response = await factory.GetAuthenticatedClient(Guid.NewGuid()).Get(
                $"/orders/{order.Id}");

            response.StatusCode.ShouldBe(403);
        }
    }
}
