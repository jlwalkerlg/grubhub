using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace WebTests.Features
{
    public class AuthenticationTests : ActionTestBase
    {
        public AuthenticationTests(ActionTestWebApplicationFactory factory) : base(factory)
        {
        }

        [Theory]
        [InlineData("GET", "/restaurants/015caf13-8252-476b-9e7f-c43767998c01/basket")]
        [InlineData("POST", "/restaurants/015caf13-8252-476b-9e7f-c43767998c01/basket")]
        [InlineData("DELETE", "/restaurants/015caf13-8252-476b-9e7f-c43767998c01/basket/items/015caf13-8252-476b-9e7f-c43767998c01")]
        [InlineData("PUT", "/restaurants/015caf13-8252-476b-9e7f-c43767998c01/basket/items/015caf13-8252-476b-9e7f-c43767998c01")]
        [InlineData("GET", "/restaurants/015caf13-8252-476b-9e7f-c43767998c01/billing")]
        [InlineData("GET", "/restaurants/015caf13-8252-476b-9e7f-c43767998c01/billing/onboarding/link")]
        [InlineData("POST", "/restaurants/015caf13-8252-476b-9e7f-c43767998c01/billing/setup")]
        [InlineData("POST", "/restaurants/015caf13-8252-476b-9e7f-c43767998c01/menu/categories")]
        [InlineData("POST", "/restaurants/015caf13-8252-476b-9e7f-c43767998c01/menu/categories/015caf13-8252-476b-9e7f-c43767998c01/items")]
        [InlineData("DELETE", "/restaurants/015caf13-8252-476b-9e7f-c43767998c01/menu/categories/015caf13-8252-476b-9e7f-c43767998c01")]
        [InlineData("DELETE", "/restaurants/015caf13-8252-476b-9e7f-c43767998c01/menu/categories/015caf13-8252-476b-9e7f-c43767998c01/items/015caf13-8252-476b-9e7f-c43767998c01")]
        [InlineData("PUT", "/restaurants/015caf13-8252-476b-9e7f-c43767998c01/menu/categories/015caf13-8252-476b-9e7f-c43767998c01")]
        [InlineData("PUT", "/restaurants/015caf13-8252-476b-9e7f-c43767998c01/menu/categories/015caf13-8252-476b-9e7f-c43767998c01/items/015caf13-8252-476b-9e7f-c43767998c01")]
        [InlineData("PUT", "/orders/015caf13-8252-476b-9e7f-c43767998c01/accept")]
        [InlineData("PUT", "/orders/015caf13-8252-476b-9e7f-c43767998c01/deliver")]
        [InlineData("GET", "/restaurant/active-orders")]
        [InlineData("GET", "/orders/015caf13-8252-476b-9e7f-c43767998c01")]
        [InlineData("POST", "/restaurants/015caf13-8252-476b-9e7f-c43767998c01/orders")]
        [InlineData("PUT", "/orders/015caf13-8252-476b-9e7f-c43767998c01/reject")]
        [InlineData("PUT", "/restaurants/015caf13-8252-476b-9e7f-c43767998c01/cuisines")]
        [InlineData("PUT", "/restaurants/015caf13-8252-476b-9e7f-c43767998c01/opening-times")]
        [InlineData("PUT", "/restaurants/015caf13-8252-476b-9e7f-c43767998c01")]
        [InlineData("GET", "/auth/user")]
        [InlineData("PUT", "/auth/user")]
        [InlineData("GET", "/restaurant/order-history")]
        [InlineData("PUT", "/orders/015caf13-8252-476b-9e7f-c43767998c01/cancel")]
        public async Task It_Requires_Authentication(string method, string uri)
        {
            var response = await GetClient().Send(
                new HttpRequestMessage(new HttpMethod(method), uri),
                new object());

            response.StatusCode.ShouldBe(401);
        }
    }
}
