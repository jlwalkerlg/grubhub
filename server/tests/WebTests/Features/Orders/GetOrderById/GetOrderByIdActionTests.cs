using System;
using System.Threading.Tasks;
using Xunit;

namespace WebTests.Features.Orders.GetOrderById
{
    public class GetOrderByIdActionTests : ActionTestBase
    {
        public GetOrderByIdActionTests(ActionTestWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task It_Fails_If_The_User_Is_Unauthenticated()
        {
            var response = await factory.GetClient().Get(
                $"/orders/{Guid.NewGuid()}");

            response.StatusCode.ShouldBe(401);
        }
    }
}
