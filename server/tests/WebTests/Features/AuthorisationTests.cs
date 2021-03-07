using System.Net.Http;
using System.Threading.Tasks;
using Web.Domain.Users;
using Xunit;

namespace WebTests.Features
{
    public class AuthorisationTests : ActionTestBase
    {
        public AuthorisationTests(ActionTestWebApplicationFactory factory) : base(factory)
        {
        }

        [Theory]
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
        [InlineData("PUT", "/orders/015caf13-8252-476b-9e7f-c43767998c01/reject")]
        [InlineData("GET", "/restaurant/active-orders")]
        [InlineData("PUT", "/restaurants/015caf13-8252-476b-9e7f-c43767998c01/cuisines")]
        [InlineData("PUT", "/restaurants/015caf13-8252-476b-9e7f-c43767998c01/opening-times")]
        [InlineData("PUT", "/restaurants/015caf13-8252-476b-9e7f-c43767998c01")]
        [InlineData("PUT", "/orders/015caf13-8252-476b-9e7f-c43767998c01/cancel")]
        public async Task It_Requires_Authentication(string method, string uri, UserRole role = UserRole.Customer)
        {
            var response = await factory.GetAuthenticatedClient(role).Send(
                new HttpRequestMessage(new HttpMethod(method), uri),
                new object());

            response.StatusCode.ShouldBe(403);
        }
    }
}
